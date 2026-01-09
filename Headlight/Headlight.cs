using MSCLoader;

using HutongGames.PlayMaker;
using UnityEngine;

namespace Headlight {
    public class Headlight : Mod
    {
        public override string ID => "Kru_Headlight"; //Your mod ID (unique)
        public override string Name => "HeadLight"; //You mod name
        public override string Author => "Krutonium"; //Your Username
        public override string Version => "1.0"; //Version
        public override string Description => "Adds a Configurable Flashlight to your Head for those Dark Finnish Nights..."; //Short description of your mod
        public override Game SupportedGames => Game.MySummerCar_And_MyWinterCar; //Supported Games

        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.ModSettings, Mod_Settings);
            SetupFunction(Setup.Update, Mod_Update);
        }

        private void Mod_Update()
        {
            if (lightToggle.GetKeybindDown())
            {
                applyButton();
                light.enabled = !light.enabled;
            }
        }

        SettingsSlider lightFoV;
        SettingsSlider lightIntensity;
        SettingsColorPicker lightColor;
        SettingsKeybind lightToggle;
        SettingsSlider lightDistance;
        
        private void Mod_Settings()
        {
            lightFoV = Settings.AddSlider("lightFoV", "FoV of the Flashlight", 0f, 120f, 50f);
            lightColor = Settings.AddColorPickerRGB("lightColor", "Color for the Flashlight", defaultColor:Color.white);
            lightIntensity = Settings.AddSlider("lightIntensity", "Intensity of the Flashlight", 0f, 10f, 2f);
            lightDistance = Settings.AddSlider("lightDistance", "How far should the light go", 0f, 100f, 30f);
            lightToggle = Keybind.Add("flashlightToggle", "Toggle Flashlight", KeyCode.Alpha3);
            Settings.AddButton("Apply", applyButton, true);
        }

        private GameObject headLight;
        private Light light;
        private void applyButton()
        {
            light.color = lightColor.GetValue();
            light.spotAngle = lightFoV.GetValue();
            light.intensity = lightIntensity.GetValue();
            light.type = LightType.Spot;
            light.range = lightDistance.GetValue();
        }
        private void Mod_OnLoad()
        {
            // Create the light object
            headLight = new GameObject("PlayerHeadlight");
            light = headLight.AddComponent<Light>();
            // Configure light settings
            applyButton();
            light.enabled = false; // Start turned off
            // Parent it to the player's camera so it follows where they look
            var playerCamera =
                (FsmVariables.GlobalVariables.FindFsmGameObject("POV").Value);
            if (playerCamera != null)
            {
                ModConsole.Print(playerCamera.name);
                headLight.transform.SetParent(playerCamera.transform);
                headLight.transform.localPosition = Vector3.zero;
                headLight.transform.localRotation = Quaternion.identity;
            }
            else
            {
                ModConsole.Print("Headlight Mod: Could not find MainCamera!");
            }
        }
    }
}