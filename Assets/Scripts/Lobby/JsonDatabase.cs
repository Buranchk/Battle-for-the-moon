using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JsonDatabase : MonoBehaviour
{
    string[] lobbyNames;
    private System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
    lobbyNames = new string[]
{
    "StarCommand",
    "OrbitOutlaw",
    "GalaxyGuard",
    "CosmoCrusade",
    "AstroSquad",
    "SpaceFleet",
    "NebulaKnite",
    "QuasarQuest",
    "CelestialCen",
    "SatelliteSnt",
    "LunarLegion",
    "ExoExplorer",
    "NovaSeekers",
    "BlackholeBan",
    "StarGuardian",
    "MoonBaseOne",
    "SolarSentry",
    "MeteorMight",
    "GalaxGuard",
    "NovaNebula",
    "QuasarQuest",
    "OrbitOracle",
    "StarStrike",
    "SunSeekers",
    "PlutoPirates",
    "SpaceSpartan",
    "LunarLancer",
    "CelestCrew",
    "NebulaNinja",
    "StarSavages",
    "GalaxyGangs",
    "AstroAces",
    "SunSoldiers",
    "StarSpecter",
    "LunarLords",
    "OrbitOmega",
    "NebulaNomad",
    "GalaxGlory",
    "MarsMarauds",
    "NovaNinjas",
    "StarSpartan",
    "LunarLynx",
    "CelestCent",
    "SolarSavant",
    "AstroAttack",
    "MarsMercs",
    "StarSheriff",
    "NovaNomads",
    "SolarSentry",
    "GalaxGoblin",
    "OrbitOrcs",
    "StarStalker",
    "SpaceSparks",
    "StarStealth",
    "NebulaNova",
    "SunSerpents",
    "MoonMystic",
    "OrbitOutfit",
    "PlutoPaladn",
    "AstroAmigos",
    "StarShootrs",
    "CelestCybgs",
    "NebulaNecro",
    "GalaxGunner",
    "PlutoProwlr",
    "MarsMarshal",
    "OrbitOtters",
    "NovaNebula",
    "SolarSerpnt",
    "AstroArgos",
    "SpaceShadow",
    "MoonMasters",
    "PlutoPulser",
    "SolarStingr",
    "GalaxGale",
    "CelestCaval",
    "OrbitOmega",
    "NovaNomad",
    "StarSerpent",
    "PlutoPirate",
    "SolarSamuri",
    "AstroAbyss",
    "NebulaNexus",
    "MoonMerc",
    "StarSerpnts",
    "SolarSlicer",
    "NovaNaga",
    "GalaxGuardn",
    "AstroAsylm",
    "OrbitOracle",
    "SpaceSlicer",
    "LunarLynx",
    "StarSpartan",
    "GalaxGale",
    "AstroArgos",
    "SolarSentry",
    "NebulaNomad",
    "MoonMaraudr",
    "GalaxGuard"
};

// Note: The length of each name is less than 15 characters.

    }

    public string ReturnName()
    {
        int randomIndex = random.Next(lobbyNames.Length);
        return lobbyNames[randomIndex];
    }

}
