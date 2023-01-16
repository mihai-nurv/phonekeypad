namespace PhoneKeypad;

internal class TranslateKeypad
{
    ///this is a mapping of the phone keypad over the characters that needs to be returned
    /// the first dictionary will tell us the charset to use. Other charsets can be added
    ///the second dictionary will have the connection between the key and the array list of possible values
    static readonly Dictionary<string, Dictionary<char, char[]>> keypad =
        new Dictionary<string, Dictionary<char, char[]>>()
        {{
            "en",
            new()
            {
                { '1', new[] { '&', '\'', '(' } },
                { '2', new[] { 'A', 'B', 'C' } },
                { '3', new[] { 'D', 'E', 'F' } },
                { '4', new[] { 'G', 'H', 'I' } },
                { '5', new[] { 'J', 'K', 'L' } },
                { '6', new[] { 'M', 'N', 'O' } },
                { '7', new[] { 'P', 'Q', 'R', 'S' } },
                { '8', new[] { 'T', 'U', 'V' } },
                { '9', new[] { 'W', 'X', 'Y', 'Z' } },
            }
        },
        {
            "ro",
            new()
            {
                { '1', new[] { '&', '\'', '(' } },
                { '2', new[] { 'A', 'B', 'C', 'Ă', 'Â' } },
                { '3', new[] { 'D', 'E', 'F' } },
                { '4', new[] { 'G', 'H', 'I', 'Î' } },
                { '5', new[] { 'J', 'K', 'L' } },
                { '6', new[] { 'M', 'N', 'O' } },
                { '7', new[] { 'P', 'Q', 'R', 'S', 'Ș', 'Ț' } },
                { '8', new[] { 'T', 'U', 'V' } },
                { '9', new[] { 'W', 'X', 'Y', 'Z' } },
            }
        }};

    public static Dictionary<char, char[]> SelectCharset(string charset)
    {
        if (keypad.ContainsKey(charset))
        {
            return keypad[charset];
        }
        throw new ArgumentOutOfRangeException($"charset {charset} was not found.");
    }

    public static string OldPhonePad(string input, string charset = "en")
    {
        StringBuilder response = new StringBuilder();

        Dictionary<char, char[]> keys = SelectCharset(charset);

        int keyPressed = 0;
        char currentKey = '\0';

        // In .NET 6 foreach is faster then index access.
        // In some situation setting the string to ToCharArray will make it faster,
        // but because we do not have big strings we will just do it on string
        foreach (char c in input)
        {
            if (c == currentKey)
                //we have the same key so we will move up the possible values
            {
                keyPressed++;
                continue;
            }

            if (currentKey == '\0') 
                //we had a space or is the start of all this so we just reset the index and move forward
            {
                currentKey = c;
                keyPressed = 0;
                continue;
            }

            if (c == '*') //handel the delete
            {
                currentKey = '\0';
                keyPressed = 0;
                continue;
            }

            //here we are definatly need to add it to response 
            //the currentKey was pressed for keyPressed times
            //get the possible values
            var possibleValues = keys[currentKey];
            //get the value needed. 
            //make sure we loop the values by dividing with the number of possibilities 
            response.Append(possibleValues[keyPressed % possibleValues.Length]);


            if (c == '#') 
                //the string should be ended but we may have some more noise so we just break when we find #
            {
                break;
            }
            if (c == ' ') 
                //if this is a space we ignore it
            {
                keyPressed = 0;
                currentKey = '\0';
            }
            else
            {
                keyPressed = 0;
                currentKey = c;
            }
        }

        return response.ToString();
    }
}
