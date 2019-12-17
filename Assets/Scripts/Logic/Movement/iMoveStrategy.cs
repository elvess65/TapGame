namespace WhaleAppTapGame.Logic.Movement
{
    public interface iMoveStrategy
    {
        event System.Action OnOutOfBottomBound;

        void Move();
    }
}
