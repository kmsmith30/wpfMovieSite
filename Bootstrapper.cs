using Caliburn.Micro;

using System.Windows;
using System.Windows.Input;

using Kvfx.ViewModels;

namespace Kvfx
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override void Configure()
        {
            MessageBinder.SpecialValues.Add("$mousepoint", ctx =>
            {
                var e = ctx.EventArgs as MouseEventArgs;

                if (e == null)
                {
                    return null;
                }

                return e.GetPosition(ctx.Source);
            });

            MessageBinder.SpecialValues.Add("$pressedkey", ctx =>
            {
                var e = ctx.EventArgs as KeyEventArgs;

                if (e == null)
                {
                    return null;
                }

                return e.Key;
            });
        }
    }
}
