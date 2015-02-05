using System;
using System.Threading;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

public class MontyHall
	{
	FormA _formA = null;
	FormB _formB = null;

	public static void Main()
		{
		Form _formA = new FormA();
		Form _formB = new FormB();

		
		_formB.Hide();
		_formA.ShowDialog();
		}
	}

public class FormA : Form
	{
	// Private members to hold references to the form's controls.
	private Button _buttonGo;
	private Label _introText;
	public event Action<object, System.EventArgs> GoClicked;

	// Constructor for the intro screen form.
	public FormA()
		{
		// Instantiate the form's control elements.
		_buttonGo = new Button();
		_introText = new Label();

		// Suspend the layout logic until form configuration completes
		SuspendLayout();

		// Configures buttonGo, which the user presses to show FormB, the game form.
		_buttonGo.Name = "buttonGo";
		_buttonGo.Text = "Let's begin!";
		_buttonGo.Location = new System.Drawing.Point(109, 80);
		_buttonGo.Click += new System.EventHandler(this.buttonGo_Click);

		// Configures introText, which displays the opening message of the intro form.
		_introText.Text = "Welcome to the Monty Hall problem!";
		_introText.Location = new System.Drawing.Point(152, 32);
		_introText.Visible = true;

		ResumeLayout(false);
		}

	// Hides the intro form and shows the game form.
	private void buttonGo_Click(object sender, System.EventArgs e)
		{
		
		}
	}

public class FormB : Form
	{
	// Private members to hold references to the form's controls.
	private Button buttonDoor1;
	private Button buttonDoor2;
	private Button buttonDoor3;
	private Boolean doorSelect1;
	private Boolean doorSelect2;
	private Boolean doorSelect3;
	private Boolean doorSwitch;
	private Boolean doorDisplay1;
	private Boolean doorDisplay2;
	private Boolean doorDisplay3;
	private Label gameText;

	// Constructor for the game screen form.
	public FormB()
		{
		// Instantiate the buttons and labels on this form.
		this.buttonDoor1 = new Button();
		this.buttonDoor2 = new Button();
		this.buttonDoor3 = new Button();
		this.gameText = new Label();

		// Suspend the layout logic until form configuration completes
		this.SuspendLayout();

		this.buttonDoor1.Name = "buttonDoor1";
		// If door 1 has been selected, then the font is bolded. Otherwise, it is normal.
		if (this.doorSelect1 == false)
			this.buttonDoor1.Font = new Font(this.Font, FontStyle.Regular);
		else
			this.buttonDoor1.Font = new Font(this.Font, FontStyle.Bold);
		// If door 1 has been opened to display a goat, change the text on the button. Otherwise, show Door 1.
		if (this.doorDisplay1 == false)
			this.buttonDoor1.Text = "Door 1";
		else
			this.buttonDoor1.Text = "A goat!";

		//Call the Door1.Click method.
		this.buttonDoor1.Click += new System.EventHandler(this.buttonDoor_Click1);

		this.buttonDoor2.Name = "buttonDoor2";
		// If door 2 has been selected, then the font is bolded. Otherwise, it is normal.
		if (this.doorSelect2.Equals(false))
			this.buttonDoor2.Font = new Font(this.Font, FontStyle.Regular);
		else
			this.buttonDoor2.Font = new Font(this.Font, FontStyle.Bold);

		//Call the Door2.Click method.
		this.buttonDoor2.Click += new System.EventHandler(this.buttonDoor_Click2);

		// If door 2 has been opened to display a goat, change the text on the button. Otherwise, show Door 1.
		if (this.doorDisplay2 == false)
			this.buttonDoor2.Text = "Door 2";
		else
			this.buttonDoor2.Text = "A goat!";


		this.buttonDoor3.Name = "buttonDoor3";

		// If door 3 has been selected, then the font is bolded. Otherwise, it is normal.
		if (this.doorSelect3.Equals(false))
			this.buttonDoor3.Font = new Font(this.Font, FontStyle.Regular);
		else
			this.buttonDoor3.Font = new Font(this.Font, FontStyle.Bold);

		//Call the Door3.Click method.
		this.buttonDoor3.Click += new System.EventHandler(this.buttonDoor_Click3);

		// If door 3 has been opened to display a goat, change the text on the button. Otherwise, show Door 1.
		if (this.doorDisplay3 == false)
			this.buttonDoor3.Text = "Door 3";
		else
			this.buttonDoor3.Text = "A goat!";

		this.ResumeLayout(false);
		}

	// Opens a confirmation box with a response based on whether this is the first or second door selection
	// and whether or not the second door selection is valid.
	private void buttonDoor_Click1(object sender, System.EventArgs e)
		{
		if (doorSwitch == false)
			{
			doorSelect1 = true;
			doorSelect2 = false;
			doorSelect3 = false;
			MessageBox.Show("You have selected Door 1. Monty will now reveal which of the other two doors contains a goat.");
			}
		else
			{
			if (doorDisplay1 == true)
				MessageBox.Show("You cannot select a goat.");
			else
				doorSelect1 = true;
			doorSelect2 = false;
			doorSelect3 = false;
			MessageBox.Show("Are you sure you want to switch to Door 1?");
			//Needs a yes/no option here.
			}
		doorSwitch = true;
		}

	private void buttonDoor_Click2(object sender, System.EventArgs e)
		{
		if (doorSwitch == false)
			{
			doorSelect1 = false;
			doorSelect2 = true;
			doorSelect3 = false;
			MessageBox.Show("You have selected Door 2. Monty will now reveal which of the other two doors contains a goat.");
			}
		else
			{
			if (doorDisplay2 == true)
				MessageBox.Show("You cannot select a goat.");
			else
				doorSelect1 = false;
			doorSelect2 = true;
			doorSelect3 = false;
			MessageBox.Show("Are you sure you want to switch to Door 2?");
			//Needs a yes/no option here.
			}
		doorSwitch = true;
		}

	private void buttonDoor_Click3(object sender, System.EventArgs e)
		{
		if (doorSwitch == false)
			{
			doorSelect1 = false;
			doorSelect2 = false;
			doorSelect3 = true;
			MessageBox.Show("You have selected Door 3. Monty will now reveal which of the other two doors contains a goat.");
			}
		else
			{
			if (doorDisplay3 == true)
				MessageBox.Show("You cannot select a goat.");
			else
				doorSelect1 = false;
			doorSelect2 = false;
			doorSelect3 = true;
			MessageBox.Show("Are you sure you want to switch to Door 3?");
			//Needs a yes/no option here.
			}
		doorSwitch = true;
		}
	}