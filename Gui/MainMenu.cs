
//------------------------------------------------------------------------------

//  <auto-generated>
//      This code was generated by:
//        TerminalGuiDesigner v1.0.25.0
//      You can make changes to this file and they will not be overwritten when saving.
//  </auto-generated>
// -----------------------------------------------------------------------------
namespace OqoSim.Gui {
    using Terminal.Gui;
    
    
    public partial class MainMenu {
        
        public MainMenu() {
            InitializeComponent();

            exitButton.Clicked += () =>
            {
                Application.RequestStop();
            };
        }
    }
}
