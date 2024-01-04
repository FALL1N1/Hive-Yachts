using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Deps;
using Client.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Managers
{
    internal class YachtManager : Client
    {
        int yachtCount = 35;
        List<YachtHandler> yachts = new List<YachtHandler>();
        List<Blips> blips = new List<Blips>();
        List<string> yachtList = new List<string>();
        List<string> yachtInts = new List<string>();
        List<string> yachtLods = new List<string>();
        List<Vector3> yachtCoords = new List<Vector3>();
        List<Vector4> yachtQuat = new List<Vector4>();

        public void InitializeYachts()
        {

            log.outInfo("Loading Yacht Add-ons...");
            UpdateYachtLists();

            log.outInfo("Loading IPLs...");
            for (int q = 0; q < yachtList.Count(); q++)
            {
                API.RequestIpl(yachtList[q]);
                API.RequestIpl(yachtInts[q]);
                API.RequestIpl(yachtLods[q]);
                log.outDebug("Yacht #" + q + "'s IPL " + yachtList[q] + " is now loaded.");
            }

            log.outInfo("Spawning all yachts...");
            SpawnHandler();

            log.outInfo("All yachts spawned and customized, have fun!");
        }

        public enum Yacht_Variations : int
        {
            // apa_mp_apa_yacht_option{1|2|3}
            yacht_the_orion = 1,     // The Orion
            yacht_the_pisces = 2,    // The Pisces
            yacht_the_aquarius = 3   // The Aquarius 
        };

        public static List<string> Yacht_Lights = new List<string>()
        {
            "1a", // Gold
            "1b", // Blue
            "1c", // Pink
            "1d", // Green
            "2a", // Gold (sidebars)
            "2b", // Blue (sidebars)
            "2c", // Pink (sidebars)
            "2d"  // Green (sidebars)
        };

        public static List<string> Yacht_Rails = new List<string>()
        {
            // apa_mp_apa_yacht_o{Yacht_Variations}_rail_{Yacht_Rails}
            "a", // Silver
            "b" // Gold 
        };

        public static List<string> Yacht_Launchers = new List<string>
        {
            "apa_mp_apa_yacht_launcher_01a", // only first yacht model
            "apa_mp_apa_yacht_launcher_02a", // 2nd and third tier
        };

        public List<string> Yacht_Flags = new List<string>
        {
                "apa_prop_flag_argentina",
                "apa_prop_flag_australia",
                "apa_prop_flag_austria",
                "apa_prop_flag_belgium",
                "apa_prop_flag_brazil",
                "apa_prop_flag_canadat_yt",
                "apa_prop_flag_china",
                "apa_prop_flag_columbia",
                "apa_prop_flag_croatia",
                "apa_prop_flag_czechrep",
                "apa_prop_flag_denmark",
                "apa_prop_flag_england",
                "apa_prop_flag_eu_yt",
                "apa_prop_flag_finland",
                "apa_prop_flag_france",
                "apa_prop_flag_german_yt",
                "apa_prop_flag_hungary",
                "apa_prop_flag_ireland",
                "apa_prop_flag_israel",
                "apa_prop_flag_italy",
                "apa_prop_flag_jamaica",
                "apa_prop_flag_japan_yt",
                "apa_prop_flag_lstein",
                "apa_prop_flag_malta",
                "apa_prop_flag_mexico_yt",
                "apa_prop_flag_netherlands",
                "apa_prop_flag_newzealand",
                "apa_prop_flag_nigeria",
                "apa_prop_flag_norway",
                "apa_prop_flag_palestine",
                "apa_prop_flag_poland",
                "apa_prop_flag_portugal",
                "apa_prop_flag_puertorico",
                "apa_prop_flag_russia_yt",
                "apa_prop_flag_scotland_yt",
                "apa_prop_flag_script",
                "apa_prop_flag_slovakia",
                "apa_prop_flag_slovenia",
                "apa_prop_flag_southafrica",
                "apa_prop_flag_southkorea",
                "apa_prop_flag_spain",
                "apa_prop_flag_sweden",
                "apa_prop_flag_switzerland",
                "apa_prop_flag_turkey",
                "apa_prop_flag_uk_yt",
                "apa_prop_flag_us_yt",
                "apa_prop_flag_wales"
        };

        public List<string> Yacht_Jacuzzi_Objects = new List<string>()
        {
                "apa_mp_apa_yacht_jacuzzi_ripple1",
                "apa_mp_apa_yacht_jacuzzi_ripple2"
        };

        public List<string> Yacht_Jacuzzi_Standing_Animations = new List<string>()
            {
                // male
                "anim@amb@yacht@jacuzzi@standing@male@variation_01@",
                "anim@amb@yacht@jacuzzi@standing@male@variation_02@",


                // female
                "anim@amb@yacht@jacuzzi@standing@female@variation_01@",
                "anim@amb@yacht@jacuzzi@standing@female@variation_02@"
            };

        List<string> Yacht_Jacuzzi_Seated_Animations = new List<string>()
        {
                // male
            "anim@amb@yacht@jacuzzi@seated@male@variation_01@",
                "anim@amb@yacht@jacuzzi@seated@male@variation_02@",
                "anim@amb@yacht@jacuzzi@seated@male@variation_03@",
                "anim@amb@yacht@jacuzzi@seated@male@variation_04@",
                "anim@amb@yacht@jacuzzi@seated@male@variation_05@",


                // female
                "anim@amb@yacht@jacuzzi@seated@female@variation_01@",
                "anim@amb@yacht@jacuzzi@seated@female@variation_02@",
                "anim@amb@yacht@jacuzzi@seated@female@variation_03@",
                "anim@amb@yacht@jacuzzi@seated@female@variation_04@",
                "anim@amb@yacht@jacuzzi@seated@female@variation_05@"
            };

        List<string> Yacht_Jacuzzi_Particles = new List<string>()
        {
            "scr_apa_jacuzzi_drips",
            "scr_apa_jacuzzi_steam",
            "scr_apa_jacuzzi_steam_sp",
            "scr_apa_jacuzzi_wade"
        };

        private void SpawnHandler()
        {
            for (int x = 0; x < yachtList.Count(); x++)
            {
                // broken IDs, perhaps not existing in GTA:O as well?
                if (x == 0 || x == 7 || x == 11 || x == 13 || x == 21 || x == 26)
                    continue;
                 
                Vector3 yachtPoz = yachtCoords[x];
                Blip yachtBlip = World.CreateBlip(yachtPoz);
                yachtBlip.Sprite = (BlipSprite)455;
                yachtBlip.Color = BlipColor.MichaelBlue;
                yachtBlip.Name = "Yacht " + x; 
            }
        }

        public enum PlayerComponents
        {
            COMPONENT_PANTS = 4,
            COMPONENT_TSHIRT = 8,
            COMPONENT_VEST = 9,
            COMPONENT_SHOES = 6,
            COMPONENT_BAG = 5,
        }

        void OnJacuzziEnter(bool enter)
        {
            int ped = API.PlayerPedId();

            int drawable_comp_id_Pants = API.GetNumberOfPedDrawableVariations(ped, (int)PlayerComponents.COMPONENT_BAG);
            int drawable_comp_id_TShirt = API.GetNumberOfPedDrawableVariations(ped, (int)PlayerComponents.COMPONENT_BAG);
            int drawable_comp_id_Vest = API.GetNumberOfPedDrawableVariations(ped, (int)PlayerComponents.COMPONENT_BAG);
            int drawable_comp_id_Shoes = API.GetNumberOfPedDrawableVariations(ped, (int)PlayerComponents.COMPONENT_BAG);
            int drawable_comp_id_bag = API.GetNumberOfPedDrawableVariations(ped, (int)PlayerComponents.COMPONENT_BAG);

            int texture_comp_id_Pants = API.GetNumberOfPedTextureVariations(ped, drawable_comp_id_Pants, (int)PlayerComponents.COMPONENT_BAG);
            int texture_comp_id_TShirt = API.GetNumberOfPedTextureVariations(ped, drawable_comp_id_TShirt, (int)PlayerComponents.COMPONENT_BAG);
            int texture_comp_id_Vest = API.GetNumberOfPedTextureVariations(ped, drawable_comp_id_Vest, (int)PlayerComponents.COMPONENT_BAG);
            int texture_comp_id_Shoes = API.GetNumberOfPedTextureVariations(ped, drawable_comp_id_Shoes, (int)PlayerComponents.COMPONENT_BAG);
            int texture_comp_id_bag = API.GetNumberOfPedTextureVariations(ped, drawable_comp_id_bag, (int)PlayerComponents.COMPONENT_BAG);


            if (enter)
            {
                API.SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_PANTS, 0, 0, 0);
                API.SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_TSHIRT, 0, 0, 0);
                API.SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_VEST, 0, 0, 0);
                API.SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_SHOES, 0, 0, 0);
                API.SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_BAG, 0, 0, 0);
            }
            else
            {
                API.SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_PANTS, drawable_comp_id_Pants, texture_comp_id_Pants, 0);
                API.SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_TSHIRT, drawable_comp_id_TShirt, texture_comp_id_TShirt, 0);
                API.SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_VEST, drawable_comp_id_Vest, texture_comp_id_Vest, 0);
                API.SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_SHOES, drawable_comp_id_Shoes, texture_comp_id_Shoes, 0);
                API.SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_BAG, drawable_comp_id_bag, texture_comp_id_bag, 0);
            }
        }

        // 35 yachts, 35 interiors, 35 lods
        // 36?
        public void UpdateYachtLists()
        {
            log.outDebug("...updating yacht lists...");

            yachtList.Add("apa_yacht_grp01_1");
            yachtInts.Add("apa_yacht_grp01_1_int");
            yachtLods.Add("apa_yacht_grp01_1_lod");
            yachtCoords.Add(new Vector3(-2814.489f, 4072.740f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0.87900420f, 0.47681410f));

            yachtList.Add("apa_yacht_grp01_2");
            yachtInts.Add("apa_yacht_grp01_2_int");
            yachtLods.Add("apa_yacht_grp01_2_lod");
            yachtCoords.Add(new Vector3(-3148.37900000f, 2807.55500000f, 5.42995500f));
            yachtQuat.Add(new Vector4(0f, 0f, 0.71906690f, -0.69494090f));

            yachtList.Add("apa_yacht_grp02_1");
            yachtInts.Add("apa_yacht_grp02_1_int");
            yachtLods.Add("apa_yacht_grp02_1_lod");
            yachtCoords.Add(new Vector3(-2814.489f, 4072.740f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0.80924760f, 0.58746780f));

            yachtList.Add("apa_yacht_grp02_2");
            yachtInts.Add("apa_yacht_grp02_2_int");
            yachtLods.Add("apa_yacht_grp02_2_lod");
            yachtCoords.Add(new Vector3(-3254.552f, 3685.676f, 5.42995500f));
            yachtQuat.Add(new Vector4(0f, 0f, -0.65576280f, 0.75496690f));

            yachtList.Add("apa_yacht_grp02_3");
            yachtInts.Add("apa_yacht_grp02_3_int");
            yachtLods.Add("apa_yacht_grp02_3_lod");
            yachtCoords.Add(new Vector3(-2368.441f, 4697.874f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp03_1");
            yachtInts.Add("apa_yacht_grp03_1_int");
            yachtLods.Add("apa_yacht_grp03_1_lod");
            yachtCoords.Add(new Vector3(-3205.344f, -219.0104f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp03_2");
            yachtInts.Add("apa_yacht_grp03_2_int");
            yachtLods.Add("apa_yacht_grp03_2_lod");
            yachtCoords.Add(new Vector3(-3448.254f, 311.5018f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp03_3");
            yachtInts.Add("apa_yacht_grp03_3_int");
            yachtLods.Add("apa_yacht_grp03_3_lod");
            yachtCoords.Add(new Vector3(-1995.725f, -1523.694f, 5.42997f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp04_1");
            yachtInts.Add("apa_yacht_grp04_1_int");
            yachtLods.Add("apa_yacht_grp04_1_lod");
            yachtCoords.Add(new Vector3(-1995.725f, -1523.694f, 5.42997f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp04_2");
            yachtInts.Add("apa_yacht_grp04_2_int");
            yachtLods.Add("apa_yacht_grp04_2_lod");
            yachtCoords.Add(new Vector3(-2117.581f, -2543.346f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp04_3");
            yachtInts.Add("apa_yacht_grp04_3_int");
            yachtLods.Add("apa_yacht_grp04_3_lod");
            yachtCoords.Add(new Vector3(-1605.074f, -1872.468f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp05_1");
            yachtInts.Add("apa_yacht_grp05_1_int");
            yachtLods.Add("apa_yacht_grp05_1_lod");
            yachtCoords.Add(new Vector3(-753.0817f, -3919.068f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp05_2");
            yachtInts.Add("apa_yacht_grp05_2_int");
            yachtLods.Add("apa_yacht_grp05_2_lod");
            yachtCoords.Add(new Vector3(-351.0608f, -3553.323f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp05_3");
            yachtInts.Add("apa_yacht_grp05_2_int");
            yachtLods.Add("apa_yacht_grp05_2_lod");
            yachtCoords.Add(new Vector3(-1460.536f, -3761.467f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp06_1");
            yachtInts.Add("apa_yacht_grp06_1_int");
            yachtLods.Add("apa_yacht_grp06_1_lod");
            yachtCoords.Add(new Vector3(1546.892f, -3045.627f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp06_2");
            yachtInts.Add("apa_yacht_grp06_2_int");
            yachtLods.Add("apa_yacht_grp06_2_lod");
            yachtCoords.Add(new Vector3(2490.886f, -2428.848f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp06_3");
            yachtInts.Add("apa_yacht_grp06_3_int");
            yachtLods.Add("apa_yacht_grp06_3_lod");
            yachtCoords.Add(new Vector3(2049.79f, -2821.624f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp07_1");
            yachtInts.Add("apa_yacht_grp07_1_int");
            yachtLods.Add("apa_yacht_grp07_1_lod");
            yachtCoords.Add(new Vector3(3029.018f, -1495.702f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp07_2");
            yachtInts.Add("apa_yacht_grp07_2_int");
            yachtLods.Add("apa_yacht_grp07_2_lod");
            yachtCoords.Add(new Vector3(3021.254f, -723.3903f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp07_3");
            yachtInts.Add("apa_yacht_grp07_3_int");
            yachtLods.Add("apa_yacht_grp07_3_lod");
            yachtCoords.Add(new Vector3(2976.622f, -1994.76f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp08_1");
            yachtInts.Add("apa_yacht_grp08_1_int");
            yachtLods.Add("apa_yacht_grp08_1_lod");
            yachtCoords.Add(new Vector3(3404.51f, 1977.0440f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp08_2");
            yachtInts.Add("apa_yacht_grp08_1_int");
            yachtLods.Add("apa_yacht_grp08_1_lod");
            yachtCoords.Add(new Vector3(3411.100f, 1193.445f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp08_3");
            yachtInts.Add("apa_yacht_grp08_3_int");
            yachtLods.Add("apa_yacht_grp08_3_lod");
            yachtCoords.Add(new Vector3(3784.802f, 2548.541f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp09_1");
            yachtInts.Add("apa_yacht_grp09_1_int");
            yachtLods.Add("apa_yacht_grp09_1_lod");
            yachtCoords.Add(new Vector3(4225.0280f, 3988.002f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp09_2");
            yachtInts.Add("apa_yacht_grp09_2_int");
            yachtLods.Add("apa_yacht_grp09_2_lod");
            yachtCoords.Add(new Vector3(4250.581f, 4576.565f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp09_3");
            yachtInts.Add("apa_yacht_grp09_2_int");
            yachtLods.Add("apa_yacht_grp09_2_lod");
            yachtCoords.Add(new Vector3(4204.3560f, 3373.70f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp10_1");
            yachtInts.Add("apa_yacht_grp10_2_int");
            yachtLods.Add("apa_yacht_grp10_2_lod");
            yachtCoords.Add(new Vector3(3751.681f, 5753.501f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp10_2");
            yachtInts.Add("apa_yacht_grp10_2_int");
            yachtLods.Add("apa_yacht_grp10_2_lod");
            yachtCoords.Add(new Vector3(3490.105f, 6305.785f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp10_3");
            yachtInts.Add("apa_yacht_grp10_3_int");
            yachtLods.Add("apa_yacht_grp10_3_lod");
            yachtCoords.Add(new Vector3(3684.853f, 5212.238f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp11_1");
            yachtInts.Add("apa_yacht_grp11_1_int");
            yachtLods.Add("apa_yacht_grp11_1_lod");
            yachtCoords.Add(new Vector3(581.5955f, 7124.558f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp11_2");
            yachtInts.Add("apa_yacht_grp11_2_int");
            yachtLods.Add("apa_yacht_grp11_2_lod");
            yachtCoords.Add(new Vector3(2004.462f, 6907.157f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp11_3");
            yachtInts.Add("apa_yacht_grp11_3_int");
            yachtLods.Add("apa_yacht_grp11_3_lod");
            yachtCoords.Add(new Vector3(1396.638f, 6860.203f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp12_1");
            yachtInts.Add("apa_yacht_grp12_1_int");
            yachtLods.Add("apa_yacht_grp12_1_lod");
            yachtCoords.Add(new Vector3(-1170.690f, 5.429955f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp12_2");
            yachtInts.Add("apa_yacht_grp12_2_int");
            yachtLods.Add("apa_yacht_grp12_2_lod");
            yachtCoords.Add(new Vector3(-777.4865f, 6566.907f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp12_3");
            yachtInts.Add("apa_yacht_grp12_3_int");
            yachtLods.Add("apa_yacht_grp12_3_lod");
            yachtCoords.Add(new Vector3(-381.7739f, 6946.960f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            log.outDebug("...updating yacht lists...");
        }

        async Task UnloadYachtIPL()
        {
            await Delay(1000);

            for (int x = 0; x < yachtList.Count(); x++)
            {
                log.outDebug("remove");
                API.RemoveIpl(yachtList[x]);
                API.RemoveIpl(yachtInts[x]);
                API.RemoveIpl(yachtLods[x]);

            }

            await Delay(1000);
            log.outDebug("Cleaned up...");
        }

    }
}
