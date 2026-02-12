extends Node

signal data_received(data: String)
signal serial_connected
signal serial_disconnected

@export var port_name: String = "COM3"
@export var baud_rate: int = 9600
@export var read_interval: float = 0.005 # 5ms between reads

var serial: GdSerial
var running := false
var buffer: String = "" # Accumulate incoming bytes

func _ready():
	serial = GdSerial.new()
	serial.set_port(port_name)
	serial.set_baud_rate(baud_rate)
	serial.set_timeout(10)

	var ports = serial.list_ports()

	if port_name not in ports:
			push_warning("Serial port not found: " + port_name)
			emit_signal("serial_disconnected")
			return

	if not serial.open():
		push_warning("Could not open serial port: " + port_name)
		emit_signal("serial_disconnected")
		return

	print("Serial connected:", port_name)
	emit_signal("serial_connected")

	running = true
	_start_read_loop()



func _start_read_loop():
	_read_loop()

func _read_loop() -> void:
	await get_tree().create_timer(2.0).timeout  # Arduino reset

	while running:
		var available = serial.bytes_available()
		if available > 0:
			var bytes = serial.read(available)          # PackedByteArray
			var data = bytes.get_string_from_utf8()     # Convert to string
			buffer += data                               # Safe now

			# Split full lines
			while buffer.find("\n") != -1:
				var idx = buffer.find("\n")
				var line = buffer.substr(0, idx).strip_edges()
				buffer = buffer.substr(idx + 1, buffer.length() - idx - 1)
				if line != "":
					emit_signal("data_received", line)
					print(line)

		await get_tree().create_timer(read_interval).timeout

	# Give Arduino time to reset
	await get_tree().create_timer(2.0).timeout

	while running:
		# Read all available bytes without blocking
		var available = serial.bytes_available()
		if available > 0:
			var data = serial.read(available)
			buffer += data

			# Split full lines
			while buffer.find("\n") != -1:
				var idx = buffer.find("\n")
				var line = buffer.substr(0, idx).strip_edges()
				buffer = buffer.substr(idx + 1, buffer.length() - idx - 1)
				if line != "":
					emit_signal("data_received", line)

		await get_tree().create_timer(read_interval).timeout

func _exit_tree():
	running = false
	if serial and serial.is_open():
		serial.close()
		print("Serial port closed")
		emit_signal("serial_disconnected")
