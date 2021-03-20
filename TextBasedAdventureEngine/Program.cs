using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace TextBasedAdventureEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            //get input file
            Console.WriteLine("Text Based Adventure Engine | Enter filepath to Adventure JSON");
            string path = Console.ReadLine();
            string adventureFileContents = File.ReadAllText(path);
            //Serialize file
            AdventureFile adventure = JsonConvert.DeserializeObject<AdventureFile>(adventureFileContents);
            if (adventure.debugMode) Console.WriteLine($"[Engine-Debug] Loaded {adventure.sections.Count} sections.");
            //Open at entry point!
            int entryPointIndex = adventure.getIndexOfSectionName("_start");
            if (entryPointIndex == -1)
            {
                Console.WriteLine("[Engine-Error] Entry point _start is not defined. Exiting.");
                return;
            }

            Console.WriteLine(adventure.sections[entryPointIndex].titletext);
            int currentSessionIndex =
                adventure.getNextSectionIndex(adventure.sections[entryPointIndex], adventure.debugMode);
            //now run this loop till the adventure file says we're done!
            while (currentSessionIndex >= 0)
            {
                Console.WriteLine(adventure.sections[currentSessionIndex].titletext);
                currentSessionIndex =
                    adventure.getNextSectionIndex(adventure.sections[currentSessionIndex], adventure.debugMode);
            }

            //exits if we get negative index, which is either an error or we want to be done.
        }
    }

    class AdventureFile
    {
        public int getIndexOfSectionName(string sectionName)
        {
            for (int i = 0; i < sections.Count; i++)
            {
                if (sections[i].sectionID == sectionName)
                {
                    return i;
                }
            }

            return -1;
        }

        public int getNextSectionIndex(Section section, bool debug)
        {
            int index = -1;
            string result = "";
            //loop till we get one
            while (true)
            {
                Console.WriteLine("Choose one of the following:");
                string options = "";
                foreach (string option in section.options)
                {
                    options += option + ", ";
                }

                Console.WriteLine(options);
                result = Console.ReadLine();
                if (section.options.Contains(result))
                {
                    break;
                }
            }

            //time to nab our index!
            int optionIndex = section.options.IndexOf(result);
            string targetSectionName = section.optionSectionLinks[optionIndex];
            if (debug)
                Console.WriteLine(
                    $"[Engine-Debug] Section ID {targetSectionName} is next section. was specified at option index {optionIndex} of section {section.sectionID}");
            //"special" targets like say, the end of an adventure:
            if (targetSectionName == "_exit")
            {
                return -2;
            }

            //normal
            index = getIndexOfSectionName(targetSectionName);
            if (index == -1)
            {
                Console.WriteLine(
                    $"[Engine-Error] Section ID {targetSectionName} was not found, but was specified at option index {optionIndex} of section {section.sectionID}");
            }

            return index;
        }

        public class Section
        {
            public string sectionID { get; set; }
            public string titletext { get; set; }
            public List<string> options { get; set; }
            public List<string> optionSectionLinks { get; set; }
        }

        public bool debugMode { get; set; }
        public List<Section> sections { get; set; }
    }
}