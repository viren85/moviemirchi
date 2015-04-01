
namespace DataStoreLib.Models
{
    public class TrackEntity : TableStorageEntity
    {
        #region table members
        public const string PARTITION_KEY = "CloudMovie";

        public string UserId { get; set; }
        public string MovieId { get; set; }
        public string ArtistId { get; set; }
        public string ReviewerId { get; set; }

        #endregion

        public TrackEntity()
            : base(PARTITION_KEY, string.Empty)
        {

        }

        public TrackEntity(string rowKey)
            : base(PARTITION_KEY, rowKey)
        {
        }

        public TrackEntity(TrackEntity entity)
            : base(PARTITION_KEY, entity.RowKey)
        {
            UserId = entity.UserId;
            MovieId = entity.MovieId;
            ArtistId = entity.ArtistId;
            ReviewerId = entity.ReviewerId;
        }

        public override string GetKey()
        {
            return this.UserId;
        }

        public static TrackEntity CreateTrackEntity(
            string userId,
            string movieId,
            string artistId,
            string reviewerId
            )
        {
            var entity = new TrackEntity(userId);
            entity.RowKey = userId;
            entity.UserId = userId;
            entity.MovieId = movieId;
            entity.ArtistId = artistId;
            entity.ReviewerId = reviewerId;

            return entity;
        }

    }
}
