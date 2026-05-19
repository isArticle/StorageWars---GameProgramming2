namespace StorageWars
{
    public class UIAnimator
    {
        public CharacterState CurrentState { get; private set; } = CharacterState.Idle;
        public CharacterState OldState { get; private set; } = CharacterState.Idle;
        public float Progress { get; private set; } = 1f;

        public void Update(CharacterState newState, float deltaTime)
        {
            if (CurrentState != newState)
            {
                OldState = CurrentState;
                CurrentState = newState;
                Progress = 0f;
            }
            if (Progress < 1f)
            {
                Progress += deltaTime * 5f;
                if (Progress > 1f) Progress = 1f;
            }
        }
    }
}