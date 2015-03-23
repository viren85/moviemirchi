<Query Kind="Program">
  <Reference>E:\StockCodes\branches\StategyEvaluator\StockDataHandler\References\HtmlAgilityPack.dll</Reference>
  <Namespace>HtmlAgilityPack</Namespace>
</Query>

void Main()
{
	var url = new Tuple<string, int>[] {
		new Tuple<string, int>("http://marathimovieworld.com/moviedetail/lokmanya-ek-yug-purush.php", 1)
		, new Tuple<string, int>("http://marathimovieworld.com/moviedetail/mitwa.php", 2)
		, new Tuple<string, int>("http://marathimovieworld.com/moviedetail/chitrafit.php", 2)
		, new Tuple<string, int>("http://marathimovieworld.com/moviedetail/kuni-mulagi-deta-ka-mulagi.php", 1)
		, new Tuple<string, int>("http://marathimovieworld.com/moviedetail/sasucha-swayamvar.php", 1)
		, new Tuple<string, int>("http://marathimovieworld.com/moviedetail/prem-at-first-sight.php", 1)
		, new Tuple<string, int>("http://marathimovieworld.com/moviedetail/bugadi-maazi-sandli-ga.php", 1)
		, new Tuple<string, int>("http://marathimovieworld.com/moviedetail/aga-bai-arechyya-2.php", 1)
		, new Tuple<string, int>("http://marathimovieworld.com/moviedetail/gondan.php", 1)
		
	}.ToList();
	
	url.ForEach(FetchMovieDetails);
}

private static void FetchMovieDetails(Tuple<string, int> task) {
var url = task.Item1;
var type = task.Item2;
	"*************".Dump();
	url.Dump();
	var html = new System.Net.WebClient().DownloadString(url);
	
	        HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(html);
            if (htmlDoc.DocumentNode != null)
            {
                HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
				if(type == 1) {
				FetchMovieDetails_Type1(bodyNode);
				} else if (type == 2){
    	        FetchMovieDetails_Type2(bodyNode);
				}
            }
}

private static void FetchMovieDetails_Type2(HtmlAgilityPack.HtmlNode bodyNode)
{
				string movieName = "";
				string img = "";
				string release = "";
				string genre = "";
				string producer= "";
				string director= "";
				string writer = "";
				string cast = "";
				string music = "";
				string singer = "";
				HtmlAgilityPack.HtmlNode node;
				
				node = GetElementWithAttribute(bodyNode, "h1", "class", "entry-title");
				movieName = node.InnerText.Trim();
				movieName.Dump();
				
                var content = GetElementWithAttribute(bodyNode, "div", "class", "entry-content");
				
				Func<string, string> Clean = (inp) => {
					if(string.IsNullOrEmpty(inp)) return string.Empty;
					return Regex.Replace(inp.Split(new char[] {':'}).Last(), @"[^\u0000-\u007F]", string.Empty).Trim();
				};
				Func<IEnumerable<string>, string, string> Fetch = (coll, keyword) => {
					return Clean(coll.FirstOrDefault(l => l.Contains(keyword)));
				};
				
				var lines = new List<string>();
				var trs = content.SelectNodes("//table/tbody/tr");
				foreach(var tr in trs) {
					lines.Add(tr.InnerText);
				}

				release = 	Fetch(lines, "Release");
				genre = 	Fetch(lines, "Genre");
				producer = 	Fetch(lines, "Producer");
				director = 	Fetch(lines, "Director");
				writer = 	Fetch(lines, "Writer");
				cast = Fetch(lines, "Cast");
				music = Fetch(lines, "Music");
				singer = Fetch(lines, "Singers");
				
				release.Dump();
				genre.Dump();
				producer.Dump();
				director.Dump();
				writer.Dump();
				cast.Dump();
				music.Dump();
				singer.Dump();
}

private static void FetchMovieDetails_Type1(HtmlAgilityPack.HtmlNode bodyNode)
{
				string movieName = "";
				string img = "";
				string release = "";
				string genre = "";
				string producer= "";
				string director= "";
				string writer = "";
				string cast = "";
				string music = "";
				string singer = "";
				HtmlAgilityPack.HtmlNode node;
				
				node = GetElementWithAttribute(bodyNode, "h1", "class", "page-header");
				if(null == node){
				node = GetElementWithAttribute(bodyNode, "h1", "class", "entry-title");
				}
				if(null != node){
				movieName = node.InnerText.Trim();
				}
				movieName.Dump();
				
                var content = GetElementWithAttribute(bodyNode, "div", "class", "entry-content");
				
				var posterNode = GetElementWithAttribute(content, "div", "class", "posterimg");
				if(null == posterNode){
					posterNode = content.SelectSingleNode("./div[1]/div[1]");
				}
				if(null != posterNode){
					node = posterNode.SelectSingleNode("./a/img");
					if(null == node){
						node = posterNode.SelectSingleNode("./img");
					}
					if(null != node){
						img = SafeGetAttributeValue(node, "src");
					}
				}
				img.Dump();
				
				Func<string, string> Clean = (inp) => {
					if(string.IsNullOrEmpty(inp)) return string.Empty;
					return Regex.Replace(inp.Split(new char[] {':'}).Last(), @"[^\u0000-\u007F]", string.Empty).Trim();
				};
				Func<IEnumerable<string>, string, string> Fetch = (coll, keyword) => {
					return Clean(coll.FirstOrDefault(l => l.Contains(keyword)));
				};
				
				var str = "";
				if(null == posterNode){
				node = content.SelectSingleNode("./div[1]");
				} else {
				node = posterNode.ParentNode;
				}
				if(null == node){
					for(int i = 1; i < 10; ++i) {
					node = content.SelectSingleNode("./p["+i+"]");
					if(node != null){
					str = node.InnerText + "\n" + str;
					}
					}
				}
				string[] lines;
				if(null != node || str != ""){
					lines = (str != "" ? str : node.InnerText).Split(new string[] {"\n", "\r"}, StringSplitOptions.None);
					release = 	Fetch(lines, "Release");
					genre = 	Fetch(lines, "Genre");
					producer = 	Fetch(lines, "Producer");
					director = 	Fetch(lines, "Director");
					writer = 	Fetch(lines, "Writer");
					cast += Fetch(lines, "Cast");
					music += Fetch(lines, "Music");
				}

				release.Dump();
				genre.Dump();
				producer.Dump();
				director.Dump();
				writer.Dump();

				node = GetElementWithAttribute(content, "div", "id", "widget-tab-latest");
				if(null == node) {
					node = content.SelectSingleNode("./p[3]");
				}
				if(null!=node){
					lines = node.InnerText.Split(new string[] {"\n", "\r"}, StringSplitOptions.None);
				
					cast += "," + Fetch(lines, "Cast");
					cast = string.Join(", ", cast.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim()).Distinct());
					music += ","+ Fetch(lines, "Music");
					music = string.Join(", ", music.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim()).Distinct());
				}
				cast.Dump();
				music.Dump();
}

// Define other methods and classes here

        private static IEnumerable<HtmlAgilityPack.HtmlNode> GetElementsWithAttribute(HtmlAgilityPack.HtmlNode root, string elementName, string name, string value)
        {
            name = name.ToLower();
            value = value.ToLower();
            elementName = elementName.ToLower();

            var nodes =
                root
                    .Descendants()
                    .Where(n => n.Name == elementName &&
                        n.Attributes.Any(a => a.Name.ToLower() == name &&
                            a.Value.ToLower().Contains(value)));
            return nodes;
        }

        private static HtmlAgilityPack.HtmlNode GetElementWithAttribute(HtmlAgilityPack.HtmlNode root, string elementName, string name, string value)
        {
            var elements = GetElementsWithAttribute(root, elementName, name, value);
            var node = elements.FirstOrDefault();
            if (node == null)
            {
                Console.WriteLine("{0} for {1} with {2} is null", elementName, name, value);
            }
            return node;
        }

        private static string SafeGetAttributeValue(HtmlAgilityPack.HtmlNode htmlNode, string attr = null)
        {
            if (attr == null)
            {
                return htmlNode.GetAttributeValue("value", null);
            }
            else
            {
                var attribute = htmlNode.Attributes.FirstOrDefault(a => a.Name == attr);
                return attribute != null ? attribute.Value : null;
            }
        }