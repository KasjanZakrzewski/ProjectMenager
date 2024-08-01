namespace ProjectMenager.Models
{
    public class MyModel<T> // dwa obiekty klasy T
    {
        public T Obj1 { get; set; }
        public T Obj2 { get; set; }
    }

    public class MyModel<T, S> // dwa obiekty odpowiednio klas T i S
    {
        public T Obj1 { get; set; }
        public S Obj2 { get; set; }
    }

    public class MyModel<T, S, R> // trzy obiekty odpowiednio klas T, S iR
    {
        public T Obj1 { get; set; }
        public S Obj2 { get; set; }
        public R Obj3 { get; set; }
    }
}
