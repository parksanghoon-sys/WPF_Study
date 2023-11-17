using Microsoft.VisualStudio.TestTools.UnitTesting;
using simpleIoc;

namespace ContainerTest
{
    public class Tests
    {
        [TestClass]
        public class ContainerTests
        {
            private Container Container { get; set; }
            [TestInitialize]
            public void Initialize()
            {
                Container = new Container();
            }
            [TestMethod]
            public void SimpleReflectionConstruction()
            {
                Container.Register<IFoo>(typeof(Foo));
            }
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