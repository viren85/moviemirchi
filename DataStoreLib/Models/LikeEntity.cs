
namespace DataStoreLib.Models
{
    public class LikeEntity : TableStorageEntity
    {
        #region table members
        public const string PARTITION_KEY = "CloudMovie";

        public string UserId { get; set; }
        public string MovieId { get; set; }
        public string ArtistId { get; set; }
        public string ReviewerId { get; set; }

        #endregion

        public LikeEntity()
            : base(PARTITION_KEY, string.Empty)
        {

        }

        public LikeEntity(string rowKey)
            : base(PARTITION_KEY, rowKey)
        {
        }

        public LikeEntity(LikeEntity entity)
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

        public static LikeEntity CreateLikeEntity(
            string userId,
            string movieId,
            string artistId,
            string reviewerId
            )
        {
            var entity = new LikeEntity(userId);
            entity.RowKey = userId;
            entity.UserId = userId;
            entity.MovieId = movieId;
            entity.ArtistId = artistId;
            entity.ReviewerId = reviewerId;

            return entity;
        }

    }
}
