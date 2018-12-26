using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TellStoryTogether.Helper
{
    public class Constant
    {
        public const string CreateState = "Create";
        public const string CommentState = "Comment";
        public const string FavoriteState = "Favorite";
        
        public const string CommentAction = "Comment";
        public const string LikeAction = "Like";
        public const string UnLikeAction = "UnLike";
        public const string FavoriteAction = "Favorite";
        public const string UnFavoriteAction = "UnFavorite";
        public const string ForkAction = "Fork";
        

        public const int LikePoint = 3;
        public const int FavoritePoint = 1;
        public const int CreatePoint = 5;
        public const int ForkedPoint = 3;
    }
}