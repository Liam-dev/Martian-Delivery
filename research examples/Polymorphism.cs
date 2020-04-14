abstract class Vehicle
{
    protected string make;
    protected string model;
    public abstract void Move();
}

class Plane : Vehicle
{
    public Plane(string make, string model)
    {
        this.make = make;
        this.model = model;
    }

    private void Fly()
    {
        Console.WriteLine($"{make} {model} is flying");
    }

    public override void Move()
    {
        Fly();
    }
}