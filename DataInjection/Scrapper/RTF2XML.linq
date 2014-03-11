
// *** // 
// Converts .txt [.rtf is pasted into .txt] into .xml file
// *** //

// *** //
// Update this as required
// *** //
  //###// This the txt version of the rtf file
string file = @"C:\Users\bash\Desktop\review_next.txt";
  //###// This is the original file
string bigXmlFile = @"E:\github\movie\review-judgements.xml";
  //###// Save to this file
string saveXmlFile = @"E:\github\movie\review-judgements_out.xml";

// *** //
// Code - Read no further
// *** //
var blob = File.ReadAllText(file);

Func<string, string, string[], string> getAttribute = (curr, next, split) =>
{
    var betw = split
        .SkipWhile(s => !s.Contains(curr))
        .TakeWhile(s => !s.Contains(next))
        .Where(s => !String.IsNullOrWhiteSpace(s));
    var res = String.Join(" ", betw)
        .Split(new string[] { curr }, StringSplitOptions.None)
        .Last()
        .Trim(new char[] { ':', ' ' });
    return res;
};

// txt => xml
var moviesBlob = blob.Split(new string[] { "Movie Name:", "Movie Name :" }, StringSplitOptions.None);
var xDoc =
    new XDocument(
        new XElement("Movies",
            moviesBlob
            .Skip(1) // Hack for now
            .Select(m =>
            {
                var split = m.Split(new char[] { '\n', '\r' }, StringSplitOptions.None);
                var name = split[0].Trim();
                var link = getAttribute("Review Link", "Review Text", split);
                var review = getAttribute("Review Text", "Reviewer Name", split);
                var rName = getAttribute("Reviewer Name", "Rating", split);
                var rating = getAttribute("Rating", "Affiliation", split).Split(':').Last().Trim();
                var rAff = getAttribute("Affiliation", "Date of Review", split);
                var rDate = getAttribute("Date of Review", "Likes", split);
                var likes = getAttribute("Likes", "\n", split);
                            
                return
                    new XElement("Movie",
                        new XAttribute("Name", name),
                        new XElement("Reviews",
                            new XElement("Review",
                            new XAttribute("Name", rName),
                            new XAttribute("Link", link),
                            new XAttribute("Affiliation", rAff),
                            new XAttribute("Date", rDate),
                            new XAttribute("Likes", likes),
                            new XAttribute("Rating", rating),
                            review
                        )
                    )
                );
            })
        )
    );

// Merge => bigDoc + xDoc
var bigDoc =
    XDocument.Load(bigXmlFile);
var allElements =
    bigDoc.Root.Elements().Concat(
        xDoc.Root.Elements()
    );

// Write to new xml
int movieId = 0;
int reviewId = 0;

Func<XElement, string, string> safeAttribute = (el, attr) =>
{
    var attrib = el == null ? null : el.Attribute(attr);
    return attrib == null ? string.Empty : attrib.Value;
};

new XDocument(
    new XElement("Movies",
        allElements
        .GroupBy(el => el.Attribute("Name").Value.Trim().ToLowerInvariant())
        .Select(g =>
            new XElement("Movie",
                new XAttribute("Id", ++movieId),
                new XAttribute("Name", g.Key),
                new XElement("Reviews",
                    g.Select(m =>
                        m.Elements()
                        .Select(rr =>
                            rr.Elements()
                            .Select(r =>
                                new XElement("Review",
                                    new XAttribute("Id", ++reviewId),
                                    new XAttribute("Name", r.Attribute("Name").Value),
                                    new XAttribute("Expected", safeAttribute(r, "Expected")),
                                    new XAttribute("Link", r.Attribute("Link").Value),
                                    new XAttribute("Affiliation", r.Attribute("Affiliation").Value),
                                    new XAttribute("Date", r.Attribute("Date").Value),
                                    new XAttribute("Likes", r.Attribute("Likes").Value),
                                    new XAttribute("Rating", r.Attribute("Rating").Value),
                                    r.Value
                                )
                            )
                        )
                    )
                )
            )
        )
    )
)
.Save(saveXmlFile);
