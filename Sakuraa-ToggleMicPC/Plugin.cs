using System;
using BepInEx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utilla;

namespace Sakuraa_ToggleMicPC
{
	[BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
	public class Plugin : BaseUnityPlugin
	{
        public Text text;
        private bool ispttType = true;
        private GameObject GorillaComputer;

        void Start()
		{
			Utilla.Events.GameInitialized += OnGameInitialized;
		}

		void OnEnable()
		{
			HarmonyPatches.ApplyHarmonyPatches();
		}

		void OnDisable()
		{
			HarmonyPatches.RemoveHarmonyPatches();
		}

		void OnGameInitialized(object sender, EventArgs e)
		{
            GorillaComputer = GameObject.Find("GorillaComputer");

            if (GorillaComputer != null)
            {
                GorillaComputer.GetComponent<GorillaNetworking.GorillaComputer>().pttType = "ALL CHAT";
                Debug.Log("Push to talk: ALL CHAT");
            }
            else
            {
                Debug.LogWarning("Push to talk: GorillaComputer not found!");
            }


            GameObject canvas = new GameObject("Canvas");
            Canvas canvasComponent = canvas.AddComponent<Canvas>();
            canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();
            text = canvas.AddComponent<Text>();
            text.text = "Mic active";
            text.fontSize = 15;
            text.color = Color.green;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.alignment = TextAnchor.LowerRight;
        }

        void Update()
		{
            if (Keyboard.current.tKey.wasPressedThisFrame)
            {
                Debug.Log("T was pressed");
                OnVoiceToggle();
            }

            if (GorillaComputer != null)
            {
                if (GorillaComputer.GetComponent<GorillaNetworking.GorillaComputer>().pttType == "ALL CHAT")
                {
                    text.text = "Mic active";
                    text.color = Color.green;
                }
                else
                {
                    text.text = "Mic Muted";
                    text.color = Color.red;
                }
            }
        }

        void OnVoiceToggle()
        {
            ispttType = !ispttType;

            if (GorillaComputer != null)
            {
                GorillaComputer.GetComponent<GorillaNetworking.GorillaComputer>().pttType = ispttType ? "ALL CHAT" : "PUSH TO TALK";
                Debug.Log("Push to talk: " + (ispttType ? "ALL CHAT" : "PUSH TO TALK"));

                if (GorillaComputer.GetComponent<GorillaNetworking.GorillaComputer>().pttType == "ALL CHAT")
                {
                    text.text = "Mic active";
                    text.color = Color.green;
                }
                else
                {
                    text.text = "Mic Muted";
                    text.color = Color.red;
                }
            }
        }
    }
}
