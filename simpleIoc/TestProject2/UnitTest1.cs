using simpleIoc;

namespace TestProject2
{
    public class UnitTest1
    {
        [Fact]
        public void SimpleReflectionConstruction()
        {
            var container = new Container();
            container.Register<IFoo, Foo>().AsSingleton();

            object instance = container.CreateScope().Resolve<IFoo>();
            object instance2 = container.CreateScope().Resolve<IFoo>();
            Assert.Same(instance,instance2);
            // xUnit의 Assert.IsType을 사용하여 타입 검사
            Assert.IsType<Foo>(instance);
        }

        [Fact]
        public void RecursiveReflectionConstruction()
        {
            var container = new Container();
            container.Register<IFoo>(typeof(Foo));
            container.Register<IBar>(typeof(Bar));
            container.Register<IBaz>(typeof(Baz));

            IBaz instance = container.Resolve<IBaz>();

            Assert.IsType<Baz>(instance);
            var baz = instance as Baz;
            Assert.IsType<Bar>(baz.Bar);
            Assert.IsType<Foo>(baz.Foo);
        }
        #region Types used for tests
        interface IFoo
        {
        }

        class Foo : IFoo
        {
        }

        interface IBar
        {
        }

        class Bar : IBar
        {
            public IFoo Foo { get; set; }

            public Bar(IFoo foo)
            {
                Foo = foo;
            }
        }

        interface IBaz
        {
        }

        class Baz : IBaz
        {
            public IFoo Foo { get; set; }
            public IBar Bar { get; set; }

            public Baz(IFoo foo, IBar bar)
            {
                Foo = foo;
                Bar = bar;
            }
        }

        class SpyDisposable : IDisposable
        {
            public bool Disposed { get; private set; }

            public void Dispose() => Disposed = true;
        }
        #endregion
    }
}