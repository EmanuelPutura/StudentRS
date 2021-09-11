using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;

namespace StudentRS
{
    public partial class MainForm : Form
    {
        #region DeclarationsOfObjects
        private const string mConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MecanicaAgricolBistrita.mdf;Integrated Security = True";
        private SqlConnection mConnection = null;
        private Button[] mStartMenuButtons = null;
        private DataGridView mStudentsDataGridView = null;
        private Button mBackButton = null;
        private Button mSelectButton = null;
        private Label mClassLabel = null;
        private TextBox mClassTextBox = null;
        private Label mLastNameLabel = null;
        private Label mFirstNameLabel = null;
        private Button mAddButton = null;
        private TextBox mLastNameTextBox = null;
        private TextBox mFirstNameTextBox = null;
        private Button mDeleteButton = null;
        private Label mGradeLabel = null;
        private Label mAbsenceLabel = null;
        private TextBox mGradeTextBox = null;
        private TextBox mAbsenceTextBox = null;
        private Button mDeleteGradesButton = null;
        private Button mDeleteAbsencesButton = null;
        private ToolTip mClassFormatToolTip = null;
        private ToolTip mDeleteGradesToolTip = null;
        private ToolTip mDeleteAbsencesToolTip = null;
        #endregion

        public MainForm()
        {
            InitializeComponent();
            InitializeStartMenu();
            InitializeDatabase();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            const int defaultFormX = 100;
            const int defaultFormY = 100;

            Location = new Point(defaultFormX, defaultFormY);

            const string appName = "StudentRS";

            BackgroundImage = global::StudentRS.Properties.Resources.book;
            Icon = global::StudentRS.Properties.Resources.book_icon;
            Text = appName;

            const int mainFormWidth = 1300;
            const int mainFormHeight = 850;

            Size = new Size(mainFormWidth, mainFormHeight);
            MaximumSize = new Size(mainFormWidth, mainFormHeight);
            MinimumSize = new Size(mainFormWidth, mainFormHeight);

            DoubleBuffered = true;
        }

        private void InitializeStartMenu()
        {
            const int numberOfStartButtons = 5;
            mStartMenuButtons = new Button[numberOfStartButtons];

            string[] startButtonNames = new string[numberOfStartButtons];

            XmlDocument startButtons = new XmlDocument();
            XmlNodeList buttons = null;
            XmlNode button = null;

            try
            {
                startButtons.Load("StartButtons.xml");
            }
            catch(XmlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch(ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch(System.IO.PathTooLongException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch(System.IO.DirectoryNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch(System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch(UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch(NotSupportedException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch(System.Security.SecurityException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            try
            {
                buttons = startButtons.SelectNodes("startButtons/button");
            }
            catch(System.Xml.XPath.XPathException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            for (int i = 0; i < buttons.Count; ++i)
            {
                try
                {
                    button = buttons.Item(i).SelectSingleNode("text");
                }
                catch(System.Xml.XPath.XPathException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                startButtonNames[i] = button.InnerText;
            }

            const int startButtonX = 350;
            int startButtonY = 0;

            const int startButtonWidth = 600;
            const int startButtonHeight = 50;

            for (int i = 0; i < mStartMenuButtons.Length; ++i)
            {
                startButtonY = 250 + i * 75;
                mStartMenuButtons[i] = new Button()
                {
                    Location = new Point(startButtonX, startButtonY),
                    Size = new Size(startButtonWidth, startButtonHeight),
                    Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    Text = startButtonNames[i],
                    ForeColor = Color.LightSlateGray,
                    Visible = true
                };
                mStartMenuButtons[i].Click += OnStartButtonClicked;

                try
                {
                    Controls.Add(mStartMenuButtons[i]);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        private void InitializeDatabase()
        {
            mConnection = new SqlConnection(mConnectionString);
            try
            {
                mConnection.Open();
            }
            catch(InvalidOperationException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch(SqlException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void OnStartButtonClicked(object sender, EventArgs e)
        {
            Button buttonSender = sender as Button;
            int index = 0;

            for (int i = 0; i < mStartMenuButtons.Length; ++i)
                if (buttonSender == mStartMenuButtons[i])
                {
                    index = i;
                    break;
                }

            switch (index)
            {
                case 0:
                    HideStartButtons();
                    ConfigureOnStartButton(index, false);
                    break;
                case 1:
                    HideStartButtons();
                    ConfigureOnStartButton(index, false);
                    break;
                case 2:
                    HideStartButtons();
                    ConfigureOnStartButton(index, false);
                    break;
                case 3:
                    HideStartButtons();
                    ConfigureOnStartButton(index, false);
                    break;
                case 4:
                    Application.Exit();
                    break;
                default:
                    break;
            }
        }

        private void OnSelectButtonClicked(object sender, EventArgs e)
        {
            string className = mClassTextBox.Text;

            if (className != "")
                SelectClass(mStudentsDataGridView, className);
        }

        private void OnAddButtonClicked(object sender, EventArgs e)
        {
            Button buttonSender = sender as Button;

            string className = mClassTextBox.Text;

            if (className != "")
                if (buttonSender.Text == "Adaugati Elev") AddStudent(mStudentsDataGridView, className);
                else if (buttonSender.Text == "Adaugati") AddGradeAndAbsence(mStudentsDataGridView, className);
        }

        private void OnDeleteStudentButtonClicked(object sender, EventArgs e)
        {
            string className = mClassTextBox.Text;

            if (className != "")
                DeleteStudent(mStudentsDataGridView, className);
        }

        private void OnDeleteGradesButton(object sender, EventArgs e)
        {
            string className = mClassTextBox.Text;

            if (className != "")
                DeleteGrades(mStudentsDataGridView, className);
        }

        private void OnDeleteAbsencesButton(object sender, EventArgs e)
        {
            string className = mClassTextBox.Text;

            if (className != "")
                DeleteAbsences(mStudentsDataGridView, className);
        }

        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            InitializeStartMenu();

            #region HideVisibleControls
            SetStyle(ControlStyles.Opaque, true);
            if (mStudentsDataGridView != null) mStudentsDataGridView.Visible = false;
            if (mBackButton != null) mBackButton.Visible = false;
            if (mSelectButton != null) mSelectButton.Visible = false;
            if (mClassLabel != null) mClassLabel.Visible = false;
            if (mClassTextBox != null) mClassTextBox.Visible = false;
            if (mAddButton != null) mAddButton.Visible = false;
            if (mLastNameLabel != null) mLastNameLabel.Visible = false;
            if (mFirstNameLabel != null) mFirstNameLabel.Visible = false;
            if (mLastNameTextBox != null) mLastNameTextBox.Visible = false;
            if (mFirstNameTextBox != null) mFirstNameTextBox.Visible = false;
            if (mDeleteButton != null) mDeleteButton.Visible = false;
            if (mGradeLabel != null) mGradeLabel.Visible = false;
            if (mGradeTextBox != null) mGradeTextBox.Visible = false;
            if (mAbsenceLabel != null) mAbsenceLabel.Visible = false;
            if (mAbsenceTextBox != null) mAbsenceTextBox.Visible = false;
            if (mDeleteGradesButton != null) mDeleteGradesButton.Visible = false;
            if (mDeleteAbsencesButton != null) mDeleteAbsencesButton.Visible = false;
            SetStyle(ControlStyles.Opaque, false);
            Refresh();
            #endregion
        }

        private void HideStartButtons()
        {
            SetStyle(ControlStyles.Opaque, true);
            for (int i = 0; i < mStartMenuButtons.Length; ++i)
                mStartMenuButtons[i].Visible = false;
            SetStyle(ControlStyles.Opaque, false);
            Refresh();
        }

        private void ConfigureOnStartButton(int buttonIndex, bool configureCase)
        {
            SetStyle(ControlStyles.Opaque, true);

            #region ConfigureVariables
            int studentsDataGridViewX = 0;
            int studentsDataGridViewY = 0;

            int studentsDataGridViewWidth = 0;
            int studentsDataGridViewHeight = 0;

            int classLabelX = 0;
            int classLabelY = 0;

            int classLabelWidth = 0;
            int classLabelHeight = 0;

            int classTextBoxX = 0;
            int classTextBoxY = 0;

            int classTextBoxWidth = 0;
            int classTextBoxHeight = 0;

            int backButtonX = 0;
            int backButtonY = 0;

            int backButtonWidth = 0;
            int backButtonHeight = 0;

            int selectButtonX = 0;
            int selectButtonY = 0;

            int selectButtonWidth = 0;
            int selectButtonHeight = 0;

            int addButtonX = 0;
            int addButtonY = 0;

            int addButtonWidth = 0;
            int addButtonHeight = 0;

            int lastNameLabelX = 0;
            int lastNameLabelY = 0;

            int lastNameLabelWidth = 0;
            int lastNameLabelHeight = 0;

            int firstNameLabelX = 0;
            int firstNameLabelY = 0;

            int firstNameLabelWidth = 0;
            int firstNameLabelHeight = 0;

            int lastNameTextBoxX = 0;
            int lastNameTextBoxY = 0;

            int lastNameTextBoxWidth = 0;
            int lastNameTextBoxHeight = 0;

            int firstNameTextBoxX = 0;
            int firstNameTextBoxY = 0;

            int firstNameTextBoxWidth = 0;
            int firstNameTextBoxHeight = 0;

            int deleteButtonX = 0;
            int deleteButtonY = 0;

            int deleteButtonWidth = 0;
            int deleteButtonHeight = 0;

            int gradeLabelX = 0;
            int gradeLabelY = 0;

            int gradeLabelWidth = 0;
            int gradeLabelHeight = 0;

            int gradeTextBoxX = 0;
            int gradeTextBoxY = 0;

            int gradeTextBoxWidth = 0;
            int gradeTextBoxHeight = 0;

            int absenceLabelX = 0;
            int absenceLabelY = 0;

            int absenceLabelWidth = 0;
            int absenceLabelHeight = 0;

            int absenceTextBoxX = 0;
            int absenceTextBoxY = 0;

            int absenceTextBoxWidth = 0;
            int absenceTextBoxHeight = 0;
            #endregion

            switch (buttonIndex)
            {
                case 0:
                    #region ConfigureControls
                    studentsDataGridViewX = 50;
                    studentsDataGridViewY = 50;

                    studentsDataGridViewWidth = 1200;
                    studentsDataGridViewHeight = 500;

                    mStudentsDataGridView = new DataGridView()
                    {
                        Location = new Point(studentsDataGridViewX, studentsDataGridViewY),
                        Size = new Size(studentsDataGridViewWidth, studentsDataGridViewHeight),
                        BackgroundColor = Color.White,
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                        Visible = true
                    };

                    SetGridColumns(mStudentsDataGridView);

                    classLabelX = 100;
                    classLabelY = 600;

                    classLabelWidth = 100;
                    classLabelHeight = 50;

                    mClassLabel = new Label()
                    {
                        Location = new Point(classLabelX, classLabelY),
                        Size = new Size(classLabelWidth, classLabelHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        BackColor = Color.Transparent,
                        Text = "Clasa:",
                        Visible = true
                    };

                    classTextBoxX = 250;
                    classTextBoxY = 600;

                    classTextBoxWidth = 250;
                    classTextBoxHeight = 50;

                    mClassTextBox = new TextBox()
                    {
                        Location = new Point(classTextBoxX, classTextBoxY),
                        Size = new Size(classTextBoxWidth, classTextBoxHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        Visible = true
                    };

                    mClassFormatToolTip = new ToolTip();
                    mClassFormatToolTip.SetToolTip(mClassTextBox, "Folositi cifre romane si nu uitlizati spatii.\nExemplu: IXA");

                    backButtonX = 550;
                    backButtonY = 675;

                    backButtonWidth = 300;
                    backButtonHeight = 50;

                    mBackButton = new Button()
                    {
                        Location = new Point(backButtonX, backButtonY),
                        Size = new Size(backButtonWidth, backButtonHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        Text = "Inapoi",
                        Visible = true
                    };
                    mBackButton.Click += OnBackButtonClicked;

                    selectButtonX = 550;
                    selectButtonY = 600;

                    selectButtonWidth = 300;
                    selectButtonHeight = 50;

                    mSelectButton = new Button()
                    {
                        Location = new Point(selectButtonX, selectButtonY),
                        Size = new Size(selectButtonWidth, selectButtonHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        Text = "Selectati Clasa",
                        Visible = true
                    };
                    mSelectButton.Click += OnSelectButtonClicked;

                    try
                    {
                        Controls.Add(mStudentsDataGridView);
                        Controls.Add(mClassLabel);
                        Controls.Add(mClassTextBox);
                        Controls.Add(mBackButton);
                        Controls.Add(mSelectButton);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    #endregion
                    break;
                case 1:
                    #region ConfigureControls
                    studentsDataGridViewX = 50;
                    studentsDataGridViewY = 50;

                    studentsDataGridViewWidth = 1200;
                    studentsDataGridViewHeight = 350;

                    mStudentsDataGridView = new DataGridView()
                    {
                        Location = new Point(studentsDataGridViewX, studentsDataGridViewY),
                        Size = new Size(studentsDataGridViewWidth, studentsDataGridViewHeight),
                        BackgroundColor = Color.White,
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                        Visible = true
                    };

                    SetGridColumns(mStudentsDataGridView);

                    backButtonX = 600;
                    backButtonY = 575;

                    backButtonWidth = 300;
                    backButtonHeight = 50;

                    mBackButton = new Button()
                    {
                        Location = new Point(backButtonX, backButtonY),
                        Size = new Size(backButtonWidth, backButtonHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        Text = "Inapoi",
                        Visible = true
                    };
                    mBackButton.Click += OnBackButtonClicked;

                    addButtonX = 600;
                    addButtonY = 500;

                    addButtonWidth = 300;
                    addButtonHeight = 50;

                    mAddButton = new Button()
                    {
                        Location = new Point(addButtonX, addButtonY),
                        Size = new Size(addButtonWidth, addButtonHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        Text = "Adaugati Elev",
                        Visible = true
                    };
                    mAddButton.Click += OnAddButtonClicked;

                    lastNameLabelX = 100;
                    lastNameLabelY = 500;

                    lastNameLabelWidth = 150;
                    lastNameLabelHeight = 50;

                    mLastNameLabel = new Label()
                    {
                        Location = new Point(lastNameLabelX, lastNameLabelY),
                        Size = new Size(lastNameLabelWidth, lastNameLabelHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        BackColor = Color.Transparent,
                        Text = "Numele:",
                        Visible = true
                    };

                    firstNameLabelX = 100;
                    firstNameLabelY = 575;

                    firstNameLabelWidth = 150;
                    firstNameLabelHeight = 50;

                    mFirstNameLabel = new Label()
                    {
                        Location = new Point(firstNameLabelX, firstNameLabelY),
                        Size = new Size(firstNameLabelWidth, firstNameLabelHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        BackColor = Color.Transparent,
                        Text = "Prenumele:",
                        Visible = true
                    };

                    classLabelX = 100;
                    classLabelY = 425;

                    classLabelWidth = 150;
                    classLabelHeight = 50;

                    mClassLabel = new Label()
                    {
                        Location = new Point(classLabelX, classLabelY),
                        Size = new Size(classLabelWidth, classLabelHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        BackColor = Color.Transparent,
                        Text = "Clasa:",
                        Visible = true
                    };

                    selectButtonX = 600;
                    selectButtonY = 425;

                    selectButtonWidth = 300;
                    selectButtonHeight = 50;

                    mSelectButton = new Button()
                    {
                        Location = new Point(selectButtonX, selectButtonY),
                        Size = new Size(selectButtonWidth, selectButtonHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        Text = "Selectati Clasa",
                        Visible = true
                    };
                    mSelectButton.Click += OnSelectButtonClicked;

                    lastNameTextBoxX = 300;
                    lastNameTextBoxY = 500;

                    lastNameTextBoxWidth = 250;
                    lastNameTextBoxHeight = 50;

                    mLastNameTextBox = new TextBox()
                    {
                        Location = new Point(lastNameTextBoxX, lastNameTextBoxY),
                        Size = new Size(lastNameTextBoxWidth, lastNameTextBoxHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        Visible = true
                    };

                    firstNameTextBoxX = 300;
                    firstNameTextBoxY = 575;

                    firstNameTextBoxWidth = 250;
                    firstNameTextBoxHeight = 50;

                    mFirstNameTextBox = new TextBox()
                    {
                        Location = new Point(firstNameTextBoxX, firstNameTextBoxY),
                        Size = new Size(firstNameTextBoxWidth, firstNameTextBoxHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        Visible = true
                    };

                    classTextBoxX = 300;
                    classTextBoxY = 425;

                    classTextBoxWidth = 250;
                    classTextBoxHeight = 50;

                    mClassTextBox = new TextBox()
                    {
                        Location = new Point(classTextBoxX, classTextBoxY),
                        Size = new Size(classTextBoxWidth, classTextBoxHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        Visible = true
                    };

                    mClassFormatToolTip = new ToolTip();
                    mClassFormatToolTip.SetToolTip(mClassTextBox, "Folositi cifre romane si nu uitlizati spatii.\nExemplu: IXA");

                    try
                    {
                        Controls.Add(mStudentsDataGridView);
                        Controls.Add(mSelectButton);
                        Controls.Add(mBackButton);
                        Controls.Add(mAddButton);
                        Controls.Add(mLastNameLabel);
                        Controls.Add(mFirstNameLabel);
                        Controls.Add(mClassLabel);
                        Controls.Add(mLastNameTextBox);
                        Controls.Add(mFirstNameTextBox);
                        Controls.Add(mClassTextBox);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    #endregion
                    break;
                case 2:
                    #region ConfigureControls
                    ConfigureOnStartButton(buttonIndex - 1, true);
                    mAddButton.Visible = false;

                    deleteButtonX = 600;
                    deleteButtonY = 500;

                    deleteButtonWidth = 300;
                    deleteButtonHeight = 50;

                    mDeleteButton = new Button()
                    {
                        Location = new Point(deleteButtonX, deleteButtonY),
                        Size = new Size(deleteButtonWidth, deleteButtonHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        Text = "Stergeti Elev",
                        Visible = true
                    };
                    mDeleteButton.Click += OnDeleteStudentButtonClicked;

                    try
                    {
                        Controls.Add(mDeleteButton);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    #endregion
                    break;
                case 3:
                    #region ConfigureControls
                    ConfigureOnStartButton(buttonIndex - 2, true);

                    const int deleteGradesButtonX = 600;
                    const int deleteGradesButtonY = 500;

                    const int deleteGradesButtonWidth = 300;
                    const int deleteGradesButtonHeight = 50;

                    mDeleteGradesButton = new Button()
                    {
                        Location = new Point(deleteGradesButtonX, deleteGradesButtonY),
                        Size = new Size(deleteGradesButtonWidth, deleteGradesButtonHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        Text = "Stergeti Notele",
                        Visible = true
                    };
                    mDeleteGradesButton.Click += OnDeleteGradesButton;

                    mDeleteGradesToolTip = new ToolTip();
                    mDeleteGradesToolTip.SetToolTip(mDeleteGradesButton, "Stergeti toate notele elevului");

                    const int deleteAbsencesButtonX = 600;
                    const int deleteAbsencesButtonY = 575;

                    const int deleteAbsencesButtonWidth = 300;
                    const int deleteAbsencesButtonHeight = 50;

                    mDeleteAbsencesButton = new Button()
                    {
                        Location = new Point(deleteAbsencesButtonX, deleteAbsencesButtonY),
                        Size = new Size(deleteAbsencesButtonWidth, deleteAbsencesButtonHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        Text = "Stergeti Absentele",
                        Visible = true
                    };
                    mDeleteAbsencesButton.Click += OnDeleteAbsencesButton;

                    mDeleteAbsencesToolTip = new ToolTip();
                    mDeleteAbsencesToolTip.SetToolTip(mDeleteAbsencesButton, "Stergeti toate absentele elevului");

                    selectButtonX = 600;
                    selectButtonY = 425;

                    mSelectButton.Location = new Point(selectButtonX, selectButtonY);

                    addButtonX = 600;
                    addButtonY = 650;

                    mAddButton.Location = new Point(addButtonX, addButtonY);

                    backButtonX = 600;
                    backButtonY = 725;

                    mBackButton.Location = new Point(backButtonX, backButtonY);

                    mAddButton.Text = "Adaugati";

                    gradeLabelX = 100;
                    gradeLabelY = 650;

                    gradeLabelWidth = 150;
                    gradeLabelHeight = 50;

                    mGradeLabel = new Label()
                    {
                        Location = new Point(gradeLabelX, gradeLabelY),
                        Size = new Size(gradeLabelWidth, gradeLabelHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        BackColor = Color.Transparent,
                        Text = "Nota:",
                        Visible = true
                    };

                    absenceLabelX = 100;
                    absenceLabelY = 725;

                    absenceLabelWidth = 150;
                    absenceLabelHeight = 50;

                    mAbsenceLabel = new Label()
                    {
                        Location = new Point(absenceLabelX, absenceLabelY),
                        Size = new Size(absenceLabelWidth, absenceLabelHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        BackColor = Color.Transparent,
                        Text = "Absenta:",
                        Visible = true
                    };

                    gradeTextBoxX = 300;
                    gradeTextBoxY = 650;

                    gradeTextBoxWidth = 250;
                    gradeTextBoxHeight = 50;

                    mGradeTextBox = new TextBox()
                    {
                        Location = new Point(gradeTextBoxX, gradeTextBoxY),
                        Size = new Size(gradeTextBoxWidth, gradeTextBoxHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        Visible = true
                    };

                    absenceTextBoxX = 300;
                    absenceTextBoxY = 725;

                    absenceTextBoxWidth = 250;
                    absenceTextBoxHeight = 50;

                    mAbsenceTextBox = new TextBox()
                    {
                        Location = new Point(absenceTextBoxX, absenceTextBoxY),
                        Size = new Size(absenceTextBoxWidth, absenceTextBoxHeight),
                        Font = new Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        ForeColor = Color.DimGray,
                        Visible = true
                    };

                    try
                    {
                        Controls.Add(mDeleteGradesButton);
                        Controls.Add(mDeleteAbsencesButton);
                        Controls.Add(mGradeLabel);
                        Controls.Add(mGradeTextBox);
                        Controls.Add(mAbsenceLabel);
                        Controls.Add(mAbsenceTextBox);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    #endregion
                    break;
                default:
                    break;
            }

            if (configureCase == true) return;

            SetStyle(ControlStyles.Opaque, false);
            Refresh();
        }

        private void SetGridColumns(DataGridView dataGridView)
        {
            dataGridView.ColumnCount = 6;
            dataGridView.Columns[0].HeaderText = "ID";
            dataGridView.Columns[1].HeaderText = "Nume";
            dataGridView.Columns[2].HeaderText = "Prenume";
            dataGridView.Columns[3].HeaderText = "Clasa";
            dataGridView.Columns[4].HeaderText = "Note";
            dataGridView.Columns[5].HeaderText = "Absente";

            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 12.5F);
            dataGridView.DefaultCellStyle.Font = new Font("Times New Roman", 10F);

            SelectClass(dataGridView, "XD");
        }

        private void SelectClass(DataGridView dataGridView, string className)
        {
            SetStyle(ControlStyles.Opaque, true);

            try
            {
                mStudentsDataGridView.Rows.Clear();
            }
            catch(InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            string query = "SELECT * FROM " + className + " ORDER BY Nume";
            SqlCommand sqlCommand = new SqlCommand(query, mConnection);
            SqlDataReader sqlDataReader = null;

            try
            {
                sqlDataReader = sqlCommand.ExecuteReader();
            }
            catch(InvalidCastException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch(InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch(System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            string[] row = null;
            while (sqlDataReader.Read())
            {
                row = new string[]
                {
                    sqlDataReader[0].ToString(),
                    sqlDataReader[1].ToString(),
                    sqlDataReader[2].ToString(),
                    sqlDataReader[3].ToString(),
                    sqlDataReader[4].ToString(),
                    sqlDataReader[5].ToString()
                };

                try
                {
                    dataGridView.Rows.Add(row);
                }
                catch(ArgumentNullException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch(InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            
            sqlDataReader.Close();
            SetStyle(ControlStyles.Opaque, false);
            Refresh();
        }

        private void AddStudent(DataGridView dataGridView, string className)
        {
            int searchedID = SearchID(dataGridView);

            string query = "INSERT INTO " + className + " (ID, Nume, Prenume, Clasa) VALUES (" + (dataGridView.Rows.Count).ToString()
                + ", '" + mLastNameTextBox.Text + "', '" + mFirstNameTextBox.Text + "', '" + mClassTextBox.Text + "')";

            SqlCommand sqlCommand = new SqlCommand(query, mConnection);

            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            SelectClass(dataGridView, className);
            RefreshID(dataGridView, className, searchedID, 1);
        }

        private void DeleteStudent(DataGridView dataGridView, string className)
        {
            int searchedID = SearchID(dataGridView);

            string query = "DELETE FROM " + className + " WHERE Nume = '" + mLastNameTextBox.Text + "' AND Prenume = '" +
                mFirstNameTextBox.Text + "'";

            SqlCommand sqlCommand = new SqlCommand(query, mConnection);

            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            SelectClass(dataGridView, className);
            RefreshID(dataGridView, className, searchedID, 2);
        }

        private int SearchID(DataGridView dataGridView)
        {
            int searchedID = dataGridView.Rows.Count + 1;
            for (int i = 0; i < dataGridView.Rows.Count - 1; ++i)
                if (dataGridView[1, i].Value.ToString() == mLastNameTextBox.Text && dataGridView[2, i].Value.ToString() == mFirstNameTextBox.Text)
                {
                    if (i == 0)                            
                        searchedID = int.Parse(dataGridView[0, i].Value.ToString());
                    else if (i > 0)
                        searchedID = int.Parse(dataGridView[0, i].Value.ToString());
                    break;
                }

            return searchedID;
        }

        private void AddGradeAndAbsence(DataGridView dataGridView, string className)
        {
            int rowNumber = 0;
            foreach(DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells[1].Value.ToString() == mLastNameTextBox.Text && row.Cells[2].Value.ToString() == mFirstNameTextBox.Text)
                    break;
                rowNumber++;
            }

            string gradeQuery = null;
            string absenceQuery = null;

            if (mGradeTextBox.Text != "")
            {
                if (dataGridView[4, rowNumber].Value.ToString() == "")
                {
                    gradeQuery = "UPDATE " + className + " SET Note = '" + mGradeTextBox.Text + "' WHERE Nume = '" +
                        mLastNameTextBox.Text + "' AND Prenume = '" + mFirstNameTextBox.Text + "'";
                }
                else if (dataGridView[4, rowNumber].Value.ToString() != "")
                {
                    gradeQuery = "UPDATE " + className + " SET Note = '" + dataGridView[4, rowNumber].Value.ToString() + ", " + mGradeTextBox.Text
                        + "' WHERE Nume = '" + mLastNameTextBox.Text + "' AND Prenume = '" + mFirstNameTextBox.Text + "'";
                }

                SqlCommand sqlCommand = new SqlCommand(gradeQuery, mConnection);
                
                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                catch (InvalidCastException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (System.IO.IOException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }

            if (mAbsenceTextBox.Text != "")
            {
                if (dataGridView[5, rowNumber].Value.ToString() == "")
                {
                    absenceQuery = "UPDATE " + className + " SET Absente = '" + mAbsenceTextBox.Text + "' WHERE Nume = '" +
                        mLastNameTextBox.Text + "' AND Prenume = '" + mFirstNameTextBox.Text + "'";
                }
                else if (dataGridView[5, rowNumber].Value.ToString() != "")
                {
                    absenceQuery = "UPDATE " + className + " SET Absente = '" + dataGridView[5, rowNumber].Value.ToString() + ", " + mAbsenceTextBox.Text
                        + "' WHERE Nume = '" + mLastNameTextBox.Text + "' AND Prenume = '" + mFirstNameTextBox.Text + "'";
                }

                SqlCommand sqlCommand = new SqlCommand(absenceQuery, mConnection);

                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                catch (InvalidCastException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (System.IO.IOException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }

            SelectClass(dataGridView, className);
        }

        private void DeleteGrades(DataGridView dataGridView, string className)
        {
            string query = "UPDATE " + className + " SET Note = '' WHERE Nume = '" +
                mLastNameTextBox.Text + "' AND Prenume = '" + mFirstNameTextBox.Text + "'";

            SqlCommand sqlCommand = new SqlCommand(query, mConnection);

            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            SelectClass(dataGridView, className);
        }

        private void DeleteAbsences(DataGridView dataGridView, string className)
        {
            string query = "UPDATE " + className + " SET Absente = '' WHERE Nume = '" +
                mLastNameTextBox.Text + "' AND Prenume = '" + mFirstNameTextBox.Text + "'";

            SqlCommand sqlCommand = new SqlCommand(query, mConnection);

            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            SelectClass(dataGridView, className);
        }

        private void RefreshID(DataGridView dataGridView, string className, int searchedID, int operationCode)
        {
            if (operationCode == 1)
            {
                int newID = -1;
                for (int row = 0; row < dataGridView.Rows.Count; ++row)
                    if (dataGridView[1, row].Value.ToString() == mLastNameTextBox.Text && dataGridView[2, row].Value.ToString() == mFirstNameTextBox.Text)
                    {
                        if (row > 0)
                            newID = int.Parse(dataGridView[0, row - 1].Value.ToString());
                        else newID = -1;
                        newID++;
                        break;
                    }

                string query = null;
                SqlCommand sqlCommand = null;

                query = "UPDATE " + className + " SET ID = ID + 1 WHERE ID >= @newID";
                sqlCommand = new SqlCommand(query, mConnection);
                sqlCommand.Parameters.AddWithValue("newID", newID);

                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                catch (InvalidCastException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (System.IO.IOException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                query = "UPDATE " + className + " SET ID = @newID WHERE ID = @ID";
                sqlCommand = new SqlCommand(query, mConnection);
                sqlCommand.Parameters.AddWithValue("newID", newID);
                sqlCommand.Parameters.AddWithValue("ID", searchedID);

                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                catch (InvalidCastException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (System.IO.IOException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                SelectClass(dataGridView, className);
            }
            else if (operationCode == 2)
            {
                string query = "UPDATE " + className + " SET ID = ID - 1 WHERE ID > @ID";
                SqlCommand sqlCommand = new SqlCommand(query, mConnection);
                sqlCommand.Parameters.AddWithValue("ID", searchedID);

                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                catch (InvalidCastException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (System.IO.IOException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                SelectClass(dataGridView, className);
            }
        }
    }
}
