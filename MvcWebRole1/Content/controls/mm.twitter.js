function LoadTweets() {
    var tweetPath = "../api/Twitter";
    CallHandler(tweetPath, ShowTweets);
}

var ShowTweets = function (data) {
    var result = JSON.parse(data);
    console.log(result);

    // Prepare UI for Twitter
}