namespace ParsingPrototype
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBoxMap = new PictureBox();
            flowLayoutPanelSetup = new FlowLayoutPanel();
            buttonLoadMap = new Button();
            labelTopLeftInput = new Label();
            textBoxTLEasting = new TextBox();
            textBoxTLNorthing = new TextBox();
            labelBottomRightInput = new Label();
            textBoxBREasting = new TextBox();
            textBoxBRNorthing = new TextBox();
            buttonApplyCorners = new Button();
            labelControls = new Label();
            listBoxControls = new ListBox();
            openFileDialogMap = new OpenFileDialog();
            buttonFindRoute = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMap).BeginInit();
            flowLayoutPanelSetup.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBoxMap
            // 
            pictureBoxMap.Location = new Point(374, 63);
            pictureBoxMap.Name = "pictureBoxMap";
            pictureBoxMap.Size = new Size(1188, 840);
            pictureBoxMap.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxMap.TabIndex = 0;
            pictureBoxMap.TabStop = false;
            pictureBoxMap.Paint += pictureBoxMap_Paint;
            pictureBoxMap.MouseClick += pictureBoxMap_MouseClick;
            // 
            // flowLayoutPanelSetup
            // 
            flowLayoutPanelSetup.Controls.Add(buttonLoadMap);
            flowLayoutPanelSetup.Controls.Add(labelTopLeftInput);
            flowLayoutPanelSetup.Controls.Add(textBoxTLEasting);
            flowLayoutPanelSetup.Controls.Add(textBoxTLNorthing);
            flowLayoutPanelSetup.Controls.Add(labelBottomRightInput);
            flowLayoutPanelSetup.Controls.Add(textBoxBREasting);
            flowLayoutPanelSetup.Controls.Add(textBoxBRNorthing);
            flowLayoutPanelSetup.Controls.Add(buttonApplyCorners);
            flowLayoutPanelSetup.Location = new Point(22, 60);
            flowLayoutPanelSetup.Name = "flowLayoutPanelSetup";
            flowLayoutPanelSetup.Size = new Size(315, 351);
            flowLayoutPanelSetup.TabIndex = 1;
            // 
            // buttonLoadMap
            // 
            buttonLoadMap.Location = new Point(3, 3);
            buttonLoadMap.Name = "buttonLoadMap";
            buttonLoadMap.Size = new Size(254, 46);
            buttonLoadMap.TabIndex = 0;
            buttonLoadMap.Text = "Load map image";
            buttonLoadMap.UseVisualStyleBackColor = true;
            buttonLoadMap.Click += buttonLoadMap_Click;
            // 
            // labelTopLeftInput
            // 
            labelTopLeftInput.AutoSize = true;
            labelTopLeftInput.Location = new Point(3, 52);
            labelTopLeftInput.Name = "labelTopLeftInput";
            labelTopLeftInput.Size = new Size(184, 32);
            labelTopLeftInput.TabIndex = 2;
            labelTopLeftInput.Text = "Top Left            ";
            // 
            // textBoxTLEasting
            // 
            textBoxTLEasting.Location = new Point(3, 87);
            textBoxTLEasting.Name = "textBoxTLEasting";
            textBoxTLEasting.Size = new Size(200, 39);
            textBoxTLEasting.TabIndex = 4;
            // 
            // textBoxTLNorthing
            // 
            textBoxTLNorthing.Location = new Point(3, 132);
            textBoxTLNorthing.Name = "textBoxTLNorthing";
            textBoxTLNorthing.Size = new Size(200, 39);
            textBoxTLNorthing.TabIndex = 6;
            // 
            // labelBottomRightInput
            // 
            labelBottomRightInput.AutoSize = true;
            labelBottomRightInput.Location = new Point(3, 174);
            labelBottomRightInput.Name = "labelBottomRightInput";
            labelBottomRightInput.Size = new Size(156, 32);
            labelBottomRightInput.TabIndex = 3;
            labelBottomRightInput.Text = "Bottom Right";
            // 
            // textBoxBREasting
            // 
            textBoxBREasting.Location = new Point(3, 209);
            textBoxBREasting.Name = "textBoxBREasting";
            textBoxBREasting.Size = new Size(200, 39);
            textBoxBREasting.TabIndex = 5;
            // 
            // textBoxBRNorthing
            // 
            textBoxBRNorthing.Location = new Point(3, 254);
            textBoxBRNorthing.Name = "textBoxBRNorthing";
            textBoxBRNorthing.Size = new Size(200, 39);
            textBoxBRNorthing.TabIndex = 7;
            // 
            // buttonApplyCorners
            // 
            buttonApplyCorners.Location = new Point(3, 299);
            buttonApplyCorners.Name = "buttonApplyCorners";
            buttonApplyCorners.Size = new Size(254, 46);
            buttonApplyCorners.TabIndex = 1;
            buttonApplyCorners.Text = "Apply corners";
            buttonApplyCorners.UseVisualStyleBackColor = true;
            buttonApplyCorners.Click += buttonApplyCorners_Click;
            // 
            // labelControls
            // 
            labelControls.AutoSize = true;
            labelControls.Location = new Point(25, 475);
            labelControls.Name = "labelControls";
            labelControls.Size = new Size(103, 32);
            labelControls.TabIndex = 2;
            labelControls.Text = "Controls";
            // 
            // listBoxControls
            // 
            listBoxControls.FormattingEnabled = true;
            listBoxControls.Location = new Point(12, 510);
            listBoxControls.Name = "listBoxControls";
            listBoxControls.Size = new Size(346, 164);
            listBoxControls.TabIndex = 3;
            // 
            // openFileDialogMap
            // 
            openFileDialogMap.FileName = "openFileDialogMap";
            openFileDialogMap.Filter = "Image files|*.png;*.jpg,*.jpeg;*.bmp";
            // 
            // buttonFindRoute
            // 
            buttonFindRoute.Location = new Point(74, 760);
            buttonFindRoute.Name = "buttonFindRoute";
            buttonFindRoute.Size = new Size(150, 46);
            buttonFindRoute.TabIndex = 4;
            buttonFindRoute.Text = "Find route";
            buttonFindRoute.UseVisualStyleBackColor = true;
            buttonFindRoute.Click += buttonFindRoute_Click;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1574, 929);
            Controls.Add(buttonFindRoute);
            Controls.Add(listBoxControls);
            Controls.Add(labelControls);
            Controls.Add(flowLayoutPanelSetup);
            Controls.Add(pictureBoxMap);
            Name = "FormMain";
            Text = "Pyxis Parsing Prototype";
            ((System.ComponentModel.ISupportInitialize)pictureBoxMap).EndInit();
            flowLayoutPanelSetup.ResumeLayout(false);
            flowLayoutPanelSetup.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxMap;
        private FlowLayoutPanel flowLayoutPanelSetup;
        private Button buttonLoadMap;
        private Button buttonApplyCorners;
        private Label labelTopLeftInput;
        private Label labelBottomRightInput;
        private Label labelControls;
        private TextBox textBoxTLEasting;
        private TextBox textBoxTLNorthing;
        private TextBox textBoxBREasting;
        private TextBox textBoxBRNorthing;
        private ListBox listBoxControls;
        private OpenFileDialog openFileDialogMap;
        private Button buttonFindRoute;
    }
}
