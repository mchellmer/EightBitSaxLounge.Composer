namespace EightBitSaxLounge.Composer.Mxl.Models.MusicTheory;

public static class NoteConverter
{
    public static string TransposeNoteToRelativeMinor(string note, int keyAsFifths, bool descending = true)
    {
        var noteOctave = int.Parse(note[^1].ToString());
        var noteName = note.Substring(0, note.Length - 1);
        var noteLetter = noteName[0];
        var keyScale = ScaleGenerator.GetScale(keyAsFifths);

        int noteIndex = Array.IndexOf(keyScale, noteName);
        int newIndex = (noteIndex - 2 + keyScale.Length) % keyScale.Length;
        var newNoteName = keyScale[newIndex];
        
        if (descending && new[]{'C', 'D'}.Contains(noteLetter))
        {
            noteOctave--;
        }
        else if (!descending && !new[]{'C', 'D'}.Contains(noteLetter))
        {
            noteOctave++;
        }

        return $"{newNoteName}{noteOctave}";
    }
}