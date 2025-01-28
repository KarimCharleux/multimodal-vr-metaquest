/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Meta.WitAi;
using Meta.WitAi.Data;
using UnityEngine;

namespace Meta.Voice.Samples.LiveUnderstanding
{
    public class LiveUnderstandingColorChanger : MonoBehaviour
    {
        [Tooltip("Changes the color of all renderers inside this container")]
        [SerializeField] private Transform _container;
        
        [Tooltip("Light to control day/night")]
        [SerializeField] private Light _mainLight;
        
        [Tooltip("Rain effect GameObject")]
        [SerializeField] private GameObject _rainEffect;

        [Header("Audio Sources")]
        [SerializeField] private AudioSource _dayAudioSource;
        [SerializeField] private AudioSource _nightAudioSource;
        [SerializeField] private AudioSource _rainAudioSource;
        
        // Constants
        private const string COLOR_SET_INTENT_ID = "change_color";
        private const string COLOR_ENTITY_ID = "meteo:color";

        // Light intensities
        private const float DAY_INTENSITY = 2f;
        private const float NIGHT_INTENSITY = 0f;

        private void Start()
        {
            // Configuration par défaut : jour et soleil
            SetDayState();
        }

        private void StopAllSounds()
        {
            if (_dayAudioSource != null) _dayAudioSource.Stop();
            if (_nightAudioSource != null) _nightAudioSource.Stop();
            if (_rainAudioSource != null) _rainAudioSource.Stop();
        }

        private void SetDayState()
        {
            if (_mainLight != null)
            {
                _mainLight.intensity = DAY_INTENSITY;
                _mainLight.color = Color.white;
            }
            if (_rainEffect != null)
            {
                _rainEffect.SetActive(false);
            }
            SetColor(Color.white);
            StopAllSounds();
            if (_dayAudioSource != null)
            {
                _dayAudioSource.Play();
            }
        }
        
        // On validate callback
        public void OnValidatePartialResponse(VoiceSession sessionData)
        {
            Debug.Log("Received partial response: " + sessionData.response.GetIntentName());
            string intentName = sessionData.response.GetIntentName();
            if (string.Equals(intentName, COLOR_SET_INTENT_ID))
            {
                Debug.Log("Received color intent:" + sessionData.response.GetIntentName());
                string[] colorNames = sessionData.response.GetAllEntityValues(COLOR_ENTITY_ID);
                Debug.Log("Color names: " + string.Join(", ", colorNames));
                if (colorNames != null && colorNames.Length > 0)
                {
                    OnValidateColorSet(sessionData, colorNames[0]);
                }
            }
        }

        // Validate & set color
        public void OnValidateColorSet(VoiceSession sessionData, string command)
        {
            Debug.Log("Color command: " + command);
            switch (command.ToLower())
            {
                case "pluie":
                    if (_rainEffect != null)
                    {
                        _rainEffect.SetActive(true);
                        SetColor(Color.blue);
                        StopAllSounds();
                        if (_rainAudioSource != null)
                        {
                            _rainAudioSource.Play();
                        }
                        Debug.Log("Pluie effect activated");
                        sessionData.validResponse = true;
                    }
                    break;
                    
                case "soleil":
                    if (_rainEffect != null)
                    {
                        SetDayState();
                        _rainEffect.SetActive(false);
                        SetColor(Color.yellow);
                        Debug.Log("Soleil effect deactivated");
                        sessionData.validResponse = true;
                    }
                    break;
                    
                case "jour":
                    if (_mainLight != null)
                    {
                        SetDayState();
                        _mainLight.intensity = DAY_INTENSITY;
                        _mainLight.color = Color.white;
                        Debug.Log("Jour effect activated");
                        SetColor(Color.white);
                        
                        sessionData.validResponse = true;
                    }
                    break;
                    
                case "nuit":
                    if (_mainLight != null)
                    {
                        _mainLight.intensity = NIGHT_INTENSITY;
                        _mainLight.color = new Color(0.6f, 0.6f, 1f); // Bleuté pour la nuit
                        SetColor(new Color(0.6f, 0.6f, 1f));
                        StopAllSounds();
                        if (_nightAudioSource != null)
                        {
                            _nightAudioSource.Play();
                        }
                        Debug.Log("Nuit effect activated");
                        sessionData.validResponse = true;
                    }
                    break;
            }
        }

        // Try to get a color
        private bool TryGetColor(string colorName, out Color color)
        {
            // Check default
            if (ColorUtility.TryParseHtmlString(colorName, out color))
            {
                return true;
            }
            // Failed
            return false;
        }

        // Set color
        public void SetColor(Color newColor)
        {
            Renderer[] renderers = _container.GetComponentsInChildren<Renderer>(true);
            foreach (var r in renderers)
            {
                r.material.color = newColor;
            }
        }
    }
}
