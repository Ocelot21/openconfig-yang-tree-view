using System;

namespace openconfig_yang_tree_view.Services
{
    public static class ExtensionService
    {
        public static string TrimAllLines(this string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            string[] lines = input.Split(new[] { "\n" }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
            }   
            return string.Join(Environment.NewLine, lines);
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

            lineIndex += textBeforeQuotes.Split(new[] { "\n" }, StringSplitOptions.None).Length + extractedText.Split(new[] { "\n" }, StringSplitOptions.None).Length - 1;

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

            lineIndex += contentInsideBrackets.Split(new[] { "\n" }, StringSplitOptions.None).Length;

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
    }
}
