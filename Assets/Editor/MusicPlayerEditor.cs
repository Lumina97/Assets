using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MusicManager))]
public class MusicPlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MusicManager manager = (MusicManager)target;
        if(manager)
        {
            if (GUILayout.Button("Start Music"))
                manager.StartMusicPlay();

            if (GUILayout.Button("Skip song"))
                manager.SkipSong();

            //Cleaner editor
            GUILayout.Space(20);
            ///////////////

            if (GUILayout.Button("Pause Music"))
                manager.PauseMusicPlay();

            if (GUILayout.Button("Resume Music"))
                manager.ResumeMusicPlay();
        }
    }
}
