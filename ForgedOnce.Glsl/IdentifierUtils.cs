using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Glsl
{
    public class IdentifierUtils
    {
        public static string JoinIdentifiers(params string[] names)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var name in names)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (builder.Length == 0)
                    {
                        builder.Append(name.Substring(0, 1).ToLower());
                        if (name.Length > 1)
                        {
                            builder.Append(name.Substring(1));
                        }
                    }
                    else
                    {
                        builder.Append(name.Substring(0, 1).ToUpper());
                        if (name.Length > 1)
                        {
                            builder.Append(name.Substring(1));
                        }
                    }
                }
            }

            return builder.ToString();
        }
    }
}
