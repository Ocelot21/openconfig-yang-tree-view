using openconfig_yang_tree_view.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;

namespace openconfig_yang_tree_view.Services
{
    public static class ExtensionService
    {
        //
        //STRING MANIPULATION
        //

        public static string TrimAllLines(this string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            string[] lines = input.Split("\n", StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
            }   
            return string.Join("\n", lines);
        }

        public static IEnumerable<string> TrimAllLines(this IEnumerable<string> input) 
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            
            var inputAsList = input.ToList();

            for (int i = 0; i < inputAsList.Count; i++)
            {
                inputAsList[i] = inputAsList[i].Trim();
            }
            return inputAsList.AsEnumerable();
        }

        public static IEnumerable<string> GetObjectContent(this IEnumerable<string> lines, int lineIndex, out int index)
        {
            List<string> objectLines = new List<string>();
            var linesList = lines.ToList();
            objectLines.Add(linesList[lineIndex]);
            bool withinOutermostBrackets = false;
            int openBrackets = 0;
            if (linesList[lineIndex].Contains("{"))
            {
                withinOutermostBrackets = true;
                openBrackets++;
            }

            if (linesList[lineIndex].Contains("}"))
            {
                openBrackets--;
            }

            while (openBrackets != 0 || !withinOutermostBrackets)
            {
                lineIndex++;
                string nextLine = linesList[lineIndex].Trim();
                objectLines.Add(nextLine);
                switch (nextLine)
                {
                    case string t when t.Contains("{"):
                        withinOutermostBrackets = true;
                        openBrackets += t.Count(x => x == '{');
                        break;

                    case string t when t.Contains("}"):
                        withinOutermostBrackets = true;
                        openBrackets -= t.Count(x => x == '}');
                        break;
                }
            }

            index = lineIndex;
            return objectLines;
        }

        public static string GetMultilineText(this List<string> lines, int i, out int index)
        {
            string line = lines[i].Trim();
            int indexOfFirst = line.IndexOf("\"") + 1;
            while (indexOfFirst == 0)
            {
                i++;
                line = lines[i].Trim();
                indexOfFirst = line.IndexOf("\"") + 1;
            }

            int indexOfLast = line.IndexOf("\"", indexOfFirst);
            List<string> descriptionPreviousLine = new List<string>();
            while (indexOfLast == -1)
            {
                descriptionPreviousLine.Add(line.Replace("\"", string.Empty));
                i++;
                line = lines[i].Trim();
                indexOfLast = line.IndexOf("\"");
            }

            if (descriptionPreviousLine.Count > 0)
            {
                descriptionPreviousLine.Add(line.Substring(0, indexOfLast));
                index = i;
                return string.Join(" ", descriptionPreviousLine);
            }
            else
            {
                index = i;
                return line.Substring(indexOfFirst, indexOfLast - indexOfFirst).Trim();
            }
        }

        public static string GetTextFromNextQuotation(this string content, int index, ref int lineIndex)
        {


            int openingQuoteIndex = content.IndexOf("\"", index);

            if (openingQuoteIndex == -1)
            {
                throw new Exception("Syntax error in .yang file! Opening qoutation mark after statement not found!");
            }

            int closingQuoteIndex = content.IndexOf("\"", openingQuoteIndex + 1);
            if (closingQuoteIndex == -1)
            {
                throw new Exception("Syntax error in .yang file! Closing quotation mark not found!");
            }

            string textBeforeQuotes = content.Substring(index, openingQuoteIndex-index);
            string extractedText = content.Substring(openingQuoteIndex + 1, closingQuoteIndex - openingQuoteIndex - 1);

            lineIndex += textBeforeQuotes.Split("\n", StringSplitOptions.None).Length + extractedText.Split("\n", StringSplitOptions.None).Length - 1;

            return extractedText;
        }

        public static string GetTextFromNextBrackets(this string content, int index, ref int lineIndex)
        {
            int openBraceIndex = content.IndexOf('{', index);
            if (openBraceIndex == -1)
            {
                return string.Empty;
            }

            int closeBraceIndex = content.FindMatchingClosingBrace(openBraceIndex);
            if (closeBraceIndex == -1)
            {
                return string.Empty;
            }

            int length = closeBraceIndex - openBraceIndex - 1;
            if (length <= 0)
            {
                return string.Empty;
            }

            string contentInsideBrackets = content.Substring(openBraceIndex + 1, length);

            lineIndex += contentInsideBrackets.Split("\n", StringSplitOptions.None).Length;

            return contentInsideBrackets;
        }

    private static int FindMatchingClosingBrace(this string content, int openBraceIndex)
        {
            int braceCount = 0;
            for (int i = openBraceIndex; i < content.Length; i++)
            {
                if (content[i] == '{')
                {
                    braceCount++;
                }
                else if (content[i] == '}')
                {
                    braceCount--;
                    if (braceCount == 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        //
        //MODULES
        //

        public static void AddUsesFromAllNodes(this Module module)
        {
            List<Use> usesToAdd = new List<Use>();
            foreach (var grouping in module.Groupings)
                module.AddUsesFromGrouping(grouping);
            module.Uses = module.Uses.Distinct().ToList();
        }

        public static void AddUsesFromGrouping(this Module module, Grouping grouping)
        {
            module.Uses.AddRange(grouping.Uses);
            foreach (var container in grouping.Containers)
                module.AddUsesFromContainer(container);
        }

        public static void AddUsesFromContainer(this Module module, Container container) 
        {
            module.Uses.AddRange(container.Uses);
            foreach (var subContainer in container.Containers)
                module.AddUsesFromContainer(subContainer);
        }

        public static void AddUsesFromList(this Module module, YangList list) 
        {
            module.Uses.AddRange(list.Uses);
            foreach (var subList in list.Lists)
                module.AddUsesFromList(subList);
            foreach (var container in list.Containers)
                module.AddUsesFromContainer(container);
        }
    }
}
