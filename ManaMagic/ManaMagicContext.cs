using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Debugger;
using ManaMagic.Logging;
using ZwellTech;
using ZwellTech.Logging;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic
{
    /// <inheritdoc />
    public sealed class ManaMagicContext : ApplicationContext
    {
        public static bool DebugMode { get; private set; } = false;
        public static ManaMagicContext Current { get; } = new ManaMagicContext();

        private readonly IApplicationNavigation applicationNavigatior;

        public SecretOfManaContext Context { get; } = new SecretOfManaContext();
        public ProjectFile ProjectFile { get; } = new ProjectFile();

        /// <inheritdoc />
        public ManaMagicContext() : base()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
            //ManaMagicContext.DebugMode = true;
#endif
            MainForm form = new MainForm();
            this.MainForm = form;
            this.applicationNavigatior = form;
            this.SetLogOutputTarget(new RichTextBoxWriter(form.LoggingControl));
        }

        public void EnableLogging()
        {
            LoggerEngine.Logger.Enable();
        }

        public void SetLogOutputTarget(TextWriter writer)
        {
            LoggerEngine.Logger.Register(writer);
        }

        /// <summary>
        /// Runs the application.
        /// </summary>
        public void Run()
        {
            this.EnableLogging();
            Application.Run(this);
        }

        public bool DebugInitializeEditor()
        {
#if DEBUG
            string romPath = @"Data\Secret of Mana (USA).sfc";
            string projectPath = @"Data\Mana Magic Test Project.mmagproj";
            this.InitializeEditor(romPath, projectPath);

            //ManaMagicContext.DebugCounts();
            return true;
#else
            return false;
#endif
        }

        public void InitializeEditor(string projectPath)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            this.ProjectFile.FilePath = projectPath;
            if (!this.ProjectFile.PreLoad())
            {
                ThrowHelper.ThrowFileNotFoundException("Unable to find ROM File");
            }

            this.InitializeEditor(this.ProjectFile.RomPath, this.ProjectFile.FilePath, false);
        }

        public void InitializeEditor(string romPath, string projectPath, bool setPaths = true)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            if (setPaths)
            {
                this.ProjectFile.FilePath = projectPath;
                this.ProjectFile.RomPath = romPath;
            }

            RomFile romFile = new RomFile(this.ProjectFile.RomPath, SecretOfManaEncoding.English);

            RomReaderFactory.Initialize(romFile);
            ManaDebugger.Initialize(this.Context);

            this.Context.Initialize(romFile);
            this.Context.Load();
            LoggerEngine.Logger.LogInformation(LogComponent.Initialization, $"ROM Parsing Complete, Current Load Time: {watch.Elapsed}");

            this.ProjectFile.Load(this.Context);
            LoggerEngine.Logger.LogInformation(LogComponent.Initialization, $"Project File Loaded, Current Load Time: {watch.Elapsed}");

            watch.Stop();
            LoggerEngine.Logger.LogInformation(LogComponent.Initialization, $"Initialization Complete, Current Load Time: {watch.Elapsed}");
        }

        public void Save()
        {
            this.ProjectFile.Save(this.Context);
            LoggerEngine.Logger.LogInformation(LogComponent.FileSystem, $"Project saved to: {this.ProjectFile.FilePath}");
        }

        public void Export(string filePath)
        {
            this.ProjectFile.Export(this.Context, filePath);
            LoggerEngine.Logger.LogInformation(LogComponent.FileSystem, $"ROM exported to: {filePath}");
        }

        /// <summary>
        /// Switches the active display to the <see cref="TreeViewSection"/> specified by section.
        /// </summary>
        /// <param name="section">The <see cref="TreeViewSection"/> to switch the display to.</param>
        public void SwitchToSection(TreeViewSection section)
        {
            this.applicationNavigatior.SwitchToNode(section);
        }

        /// <summary>
        /// Switches the active display to the <see cref="TreeViewSection"/> specified by section and sets the data index to the value specified by index.
        /// </summary>
        /// <param name="section">The <see cref="TreeViewSection"/> to switch the display to.</param>
        /// <param name="index">The data index the section should display.</param>
        public void SwitchToSection(TreeViewSection section, int index)
        {
            this.applicationNavigatior.SwitchToNode(section, index);
        }

        [Conditional("DEBUG")]
        private static void DebugCounts()
        {
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintLongestEventTextLine);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintMaxSizeForItemStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintMaxSizeForSpellStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintMaxSizeForEnemyStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintSizeForWeaponDescriptionStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintSizeForSpellDescriptionStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintMaxSizeForTownNameStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintMaxSizeForItemErrorStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintSizeForShopStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintMaxSizeForStatusEffectStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintMaxSizeForElementalFearStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintMaxSizeForTrapStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintMaxSizeForWeaponMessageStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintMaxSizeForBossSkillNameStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintMaxSizeForBuffDebuffStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintMaxSizeForLunarMagicStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintMaxSizeForTreasureChestMessageStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintMaxSizeForCombatMessageStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintMaxSizeForAnalyzerMessageStrings);
            ManaDebugger.RunDebugger(TextDebuggerOption.PrintMaxSizeForLevelUpMessageStrings);
        }
    }
}