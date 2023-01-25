
public class Box : Health
{
    public override void Died()
    {
        Destroy(gameObject);
    }
}
