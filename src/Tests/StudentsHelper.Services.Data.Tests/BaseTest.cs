namespace StudentsHelper.Services.Data.Tests
{
    using System.Reflection;

    using StudentsHelper.Services.Mapping;
    using StudentsHelper.Web.ViewModels;

    public class BaseTest : IBaseTest
    {
        private static bool hasBeenUsed = false;
        private static object lockObj = new object();

        public BaseTest()
        {
            lock (lockObj)
            {
                if (!hasBeenUsed)
                {
                    AutoMapperConfig.RegisterMappings(
                                       typeof(BaseTest).GetTypeInfo().Assembly,
                                       typeof(ErrorViewModel).GetTypeInfo().Assembly);

                    hasBeenUsed = true;
                }
            }

            // What the warning warns of is actually a wanted effect
#pragma warning disable CA2214 // Do not call overridable methods in constructors
            this.CleanWorkbench();
#pragma warning restore CA2214 // Do not call overridable methods in constructors
        }

        public virtual void CleanWorkbench()
        {
        }
    }
}
