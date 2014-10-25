using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CloudMovie.APIRole.Library
{
    public class ImageGenerator
    {
        /// <summary>
        /// To test this code
        /// <code>
        ///    {
        ///        var tweets = new string[] {
        ///            "#HappyNewYear http://bit.ly/12ooKaL  collects ₹44.97 cr on opening day",
        ///            "Which moments PK smile in this trailer? http://bit.ly/1rsz46p  #PK #PKTeaser",
        ///            "Tell us who is acting maestro in #HNY SRK, Jr AB, Boman, Deepika, Sonu? http://bit.ly/12ooKaL   #MovieMirchi",
        ///            "Which is your favorite song from #HNY http://bit.ly/12ooKaL  ? Lovely, India Wale or Manwa Lage?  #MovieMirchi",
        ///            "HNY http://bit.ly/12ooKaL  set the world record by releasing in 6000 screens #HappyNewYear #MovieMirchi",
        ///            "#PK team http://bit.ly/1rsz46p  plans to hold screening for Sanjay Dutt http://bit.ly/1D5BhuV  http://dnai.in/cq7S",
        ///            "Will Happy New Year http://bit.ly/12ooKaL  make the hat trick for SRK & Farah after Main Hoon Na & Om Shanti Om? #HappyNewYear",
        ///            "Though he never sipped alcohol in his life but his name is #PK http://bit.ly/1pDAdYZ",
        ///            "Wish you very Happy Diwali #MovieMirchi",
        ///            "On this auspicious day of Diwali, we are delighted to launch our website http://MovieMirchi.co.in Lets start journey together #MovieMirchi",
        ///        };
        ///    
        ///        TweetFormatter formatter = new TweetFormatter();
        ///        var tweetLines = formatter.Format(tweets);
        ///    
        ///        ImageGenerator generator = new ImageGenerator();
        ///        generator.GenerateTweetImage(tweetLines);
        ///    }
        /// </code>
        /// The output file is named as Tweet.png
        /// </summary>

        class Formatting
        {
            public float FontSize { get; set; }
            public float Height { get; set; }
        }

        public void GenerateTweetImage(IEnumerable<string> textLines)
        {
            const int header_width = 700;
            const int header_height = 200;
            const int padding_height = 10;
            const int line_spacing = 20;
            const int footer_height = 30;
            const int bullet_left_indent = 10;
            const int tweet_left_indent = 60;
            Color textColor = Color.Chocolate;
            Color backColor = Color.Cornsilk;
            var header = new Bitmap(@"Resources/TweetImageHeader.png");
            var bullet = new Bitmap(@"Resources/favicon.png");

            int running_height = header_height + padding_height;

            Image textImage = new Bitmap(header_width, header_height + footer_height + 2 * textLines.Count() * (padding_height + 80));
            bool first = true;

            Func<bool, string, Formatting, Formatting> AddImage = (isFirstPart, text, format) =>
            {
                SizeF textSize;
                float fontSize = 37;

                if (format != null)
                {
                    fontSize = format.FontSize;
                    textSize = new SizeF(header_width, format.Height);
                }
                else
                {
                    using (Image dummyImage = new Bitmap(1, 1))
                    {
                        using (Graphics drawing = Graphics.FromImage(dummyImage))
                        {
                            do
                            {
                                fontSize -= 2;
                                var font = new Font(FontFamily.GenericSansSerif, fontSize);
                                textSize = drawing.MeasureString(text, font);

                            } while (textSize.Width > (header_width - tweet_left_indent));

                            --fontSize;
                        }
                    }
                }

                using (Graphics drawing = Graphics.FromImage(textImage))
                {
                    float y = running_height - (isFirstPart ? -line_spacing : 0);

                    if (first)
                    {
                        drawing.Clear(backColor);
                        drawing.DrawImage(header, 0, 0);
                        first = false;
                    }

                    using (Brush textBrush = new SolidBrush(textColor))
                    {
                        var font = new Font(FontFamily.GenericSansSerif, fontSize);
                        drawing.DrawString(text, font, textBrush, tweet_left_indent, y);
                    }

                    if (isFirstPart)
                    {
                        drawing.DrawImage(bullet, bullet_left_indent, y);
                    }

                    drawing.Save();
                }

                running_height += ((int)textSize.Height + (isFirstPart ? line_spacing : 0));

                return new Formatting()
                {
                    FontSize = fontSize,
                    Height = textSize.Height,
                };
            };

            foreach (string textLine in textLines)
            {
                string[] parts = textLine.Split(new string[] { "<break here>" }, StringSplitOptions.None);
                if (parts.Length == 1)
                {
                    AddImage(true, parts[0], null);
                }
                else
                {
                    Formatting format = AddImage(true, parts[0], null);
                    foreach (var part in parts.Skip(1))
                    {
                        AddImage(false, part, format);
                    }
                }
            }

            textImage.Save(@"Temp.png");

            using (var finalImage = new Bitmap(header_width, running_height + footer_height))
            {
                using (Graphics finalDrawing = Graphics.FromImage(finalImage))
                {
                    var finalCopy = new Bitmap(@"Temp.png");
                    finalDrawing.DrawImage(finalCopy, 0, 0);
                    finalDrawing.Save();
                }

                finalImage.Save(@"Tweet.png");
            }
        }
    }
}