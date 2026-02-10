import librosa
import json
import sys
from pathlib import Path

def analyze_bpm(audio_path):
    # Load audio (mono=True ensures y is 1D)
    y, sr = librosa.load(audio_path, mono=True)
    
    # Detect tempo (returns a float now)
    tempo, _ = librosa.beat.beat_track(y=y, sr=sr)
    
    # Ensure tempo is float
    if hasattr(tempo, "__len__"):
        tempo = float(tempo[0])
    
    return round(float(tempo), 2)

def main():
    if len(sys.argv) < 2:
        print("Usage: python GetBPM.py <audiofile>")
        return

    audio_file = Path(sys.argv[1])
    bpm = analyze_bpm(audio_file)

    output = {
        "file": audio_file.name,
        "bpm": bpm
    }

    output_path = audio_file.with_suffix(".json")
    with open(output_path, "w") as f:
        json.dump(output, f, indent=2)

    print(f"BPM: {bpm}")

if __name__ == "__main__":
    main()
