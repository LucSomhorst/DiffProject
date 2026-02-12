import librosa
import json
import sys

def analyze_beats(audio_path, beats_per_bar=4):
    # Load audio
    y, sr = librosa.load(audio_path, sr=None)

    # Beat tracking
    tempo, beat_frames = librosa.beat.beat_track(y=y, sr=sr)

    # Convert frames to time (seconds)
    beat_times = librosa.frames_to_time(beat_frames, sr=sr)

    # ---- BAR DETECTION (downbeats) ----
    # Assume constant meter (default: 4/4)
    bar_frames = beat_frames[::beats_per_bar]
    bar_times = beat_times[::beats_per_bar]

    return {
        "tempo_bpm": float(tempo.item()),  
        "beats_per_bar": beats_per_bar,
        "beat_frames": beat_frames.tolist(),
        "beat_times_seconds": beat_times.tolist(),
        "bar_frames": bar_frames.tolist(),
        "bar_times_seconds": bar_times.tolist()
    }

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python beat_analysis.py <audiofile>")
        sys.exit(1)

    audio_file = sys.argv[1]
    result = analyze_beats(audio_file)

    print(json.dumps(result, indent=2))
