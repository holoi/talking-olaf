using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ElevenLabs;
using ElevenLabs.TextToSpeech;
using ElevenLabs.VoiceGeneration;
using ElevenLabs.Voices;
using System.Linq;
using System;

namespace ElevenLabsService
{
    public class ElevenLabsTTS : MonoBehaviour
    {
        [SerializeField]
        AudioSource audioSource;

        [SerializeField]
        string apiKey;

        string InputText = "The quick brown fox jumps over the lazy dog.";

        ElevenLabsClient api;


        bool isStartPlayTTSAudioProcess = false;

        //events
        [SerializeField]
        UnityEvent TTSBeginEvents;
        [SerializeField]
        UnityEvent TTSEndEvents;

        // Start is called before the first frame update
        void Start()
        {
            api = new ElevenLabsClient(apiKey);

            //StreamTTS();
            //TTS();
            //GetAllVoice();
        }

        // Update is called once per frame
        void Update()
        {
            if (isStartPlayTTSAudioProcess)
            {
                if (!audioSource.isPlaying)
                {
                    isStartPlayTTSAudioProcess = false;
                    //InvokeStopEvents
                    Debug.Log("TTSEndEvents Invoke!");
                    TTSEndEvents?.Invoke();
                }
            }
        }

        // print all voice with id
        async void GetAllVoice()
        {
            var allVoices = await api.VoicesEndpoint.GetAllVoicesAsync();

            foreach (var voice in allVoices)
            {
                Debug.Log($"{voice.Id} | {voice.Name} | similarity boost: {voice.Settings?.SimilarityBoost} | stability: {voice.Settings?.Stability}");
            }
        }

        async void DefaultVoiceSettings()
        {
            var result = await api.VoicesEndpoint.GetDefaultVoiceSettingsAsync();
            Debug.Log($"stability: {result.Stability} | similarity boost: {result.SimilarityBoost}");
        }

        // using a specific voice 
        async void SpecificVoiceSettings()
        {
            /* 
             * yoZ06aMxZJJ28mfd3POQ | Sam | similarity boost: 0.75 | stability: 0.42
             * pNInz6obpgDQGcFmaJgB | Adam | similarity boost: 0.75 | stability: 0.5
             * VR6AewLTigWG4xSOukaG | Arnold | similarity boost: 0.75 | stability: 0.5
             * TxGEqnHWrfWFTfGW9XjX | Josh | similarity boost: 0.75 | stability: 0.5
             * MF3mGyEYCl7XYWbV9V6O | Elli | similarity boost: 0.75 | stability: 0.5
             * ErXwobaYiN019PkySvjV | Antoni | similarity boost: 0.75 | stability: 0.5
             * EXAVITQu4vr4xnSDxMaL | Bella | similarity boost: 0.75 | stability: 0.5
             * AZnzlk1XvdvUeBnXmlld | Domi | similarity boost: 0.75 | stability: 0.5
             * 21m00Tcm4TlvDq8ikWAM | Rachel | similarity boost: 0.75 | stability: 0.5
             * vDMb7LyBvR9IYV5tNJnV | Olaf Test 1 | similarity boost: 0.75 | stability: 0.5
             * 
            */
            var voice = await api.VoicesEndpoint.GetVoiceAsync("vDMb7LyBvR9IYV5tNJnV");
            var success = await api.VoicesEndpoint.EditVoiceSettingsAsync(voice, new VoiceSettings(0.7f, 0.7f));
            Debug.Log($"Was successful? {success}");
        }

        async void TTS()
        {
            var text = "The quick brown fox jumps over the lazy dog.";
            var voice = (await api.VoicesEndpoint.GetAllVoicesAsync()).FirstOrDefault();
            var defaultVoiceSettings = await api.VoicesEndpoint.GetDefaultVoiceSettingsAsync();
            var (clipPath, audioClip) = await api.TextToSpeechEndpoint.TextToSpeechAsync(text, voice, defaultVoiceSettings);
            Debug.Log(clipPath);
        }

        public async void StreamTTS()
        {
            TTSBeginEvents?.Invoke();
            Debug.Log("TTSBeginEvents Invoke!");

            InputText = FindObjectOfType<GameManager>().talkLine;
            var voice = (await api.VoicesEndpoint.GetVoiceAsync("yoZ06aMxZJJ28mfd3POQ"));
            var defaultVoiceSettings = await api.VoicesEndpoint.GetDefaultVoiceSettingsAsync();
            var (clipPath, audioClip) = await api.TextToSpeechEndpoint.StreamTextToSpeechAsync(
                InputText,
                voice,
                clip =>
                {
                    // Event raised as soon as the clip has loaded enough data to play.
                    // May not provide or play full clip until Unity bug is addressed.
                    audioSource.clip = clip;
                    audioSource.Play();

                    //audioSource.PlayOneShot(clip);
                    isStartPlayTTSAudioProcess = true;
                },
                defaultVoiceSettings);
            //Debug.Log(clipPath);
        }
    }


}

