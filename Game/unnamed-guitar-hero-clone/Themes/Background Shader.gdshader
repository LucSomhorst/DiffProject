shader_type canvas_item;

uniform vec4 color_a : source_color = vec4(0.29, 0.76, 0.71, 1.0); // #4BC3B5
uniform vec4 color_b : source_color = vec4(0.11, 0.17, 0.33, 1.0); // #1C2C54
uniform vec4 color_c : source_color = vec4(0.82, 0.46, 0.72, 1.0); // #D175B7

uniform float time;
uniform float speed = 0.15;
uniform float wave_strength = 0.08;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec2 uv = UV;

	float t = time * speed;

	float wave =
		sin((uv.y * 3.0  + t) * 6.0) * wave_strength +
		cos((uv.x * 2.0  - t) * 4.0) * wave_strength;

	float gradient = uv.y + wave;

	vec4 base_color = mix(color_b, color_a, gradient);
	vec4 final_color = mix(base_color, color_c, uv.x * 0.3);

	COLOR = final_color;

    //COLOR = vec4(
        //0.10 + wave,
        //0.08 + wave * 0.8,
        //0.16 + wave * 1.2,
        //1.0
    //);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
