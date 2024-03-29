﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Mackay_Nel_Task1
{
    [Serializable]
    public class Map
    {
         
        List<Building> buildings ;
        List<Unit> units = new List<Unit>();
        Random rd = new Random();
        int numUnits = 0;
        int numBuildings = 0;
        int numWizards = 0; //Added for Task 3 
        TextBox infoTxtBox = new TextBox();
        private int v;

       public List<Unit> Units
        {
            get { return units; }
            set { units = value; }
        }
        public List<Building> Buildings
        {
            get { return buildings; }
            set { buildings = value; }
        }

        public Map(int n, int bu,int wu, TextBox txt)
        {
            buildings = new List<Building>();
            numUnits = n;
            numBuildings = bu;
            numWizards = wu; //Added for Task 3 
            infoTxtBox = txt;
        }

        public void Generate()
        {
            for(int i = 0; i < numUnits; i++)//Generate Melee Unit.
            {
                if(rd.Next(0,4)== 0)
                {
                    MeleeUnit m = new MeleeUnit(rd.Next(0, 20),
                                                rd.Next(0, 20),
                                                100,
                                                1,
                                                20,
                                                (i % 2 == 0 ? 1 : 0),
                                                "M",
                                                "Knight");
                    units.Add(m);
                }
                else if (rd.Next(0, 4) == 1) // Generate Ranged Unit
                {
                    RangedUnit r = new RangedUnit(rd.Next(0, 20),
                                                 rd.Next(0, 20),
                                                 100,
                                                 1,
                                                 20,
                                                 5,
                                                 (i % 2 == 0 ? 1 : 0),
                                                 "R",
                                                 "Musketeer");// [Update]: For task 3 I changed the units from wizards to musketeers as wizards are now their own unit type.
                    units.Add(r);
                }
                else if (rd.Next(0, 4) == 2)
                {
                    WizardUnit w = new WizardUnit(rd.Next(0, 20),
                                                 rd.Next(0, 20),
                                                 100,
                                                 1,
                                                 20,
                                                 5,
                                                 (i % 2 == 0 ? 1 : 0),
                                                 "W",
                                                 "Wizard");//Task 3: Added the wizard. 
                    units.Add(w);
                }
                else //[Task 3] Generates Neutral Wizzards in their own faction. 
                {
                    RougeWizardUnit wr = new RougeWizardUnit(rd.Next(0, 20),
                                                    rd.Next(0, 20),
                                                    100,
                                                    1,
                                                    20,
                                                    5,
                                                    (i % 2 == 0 ? 1 : 0),
                                                    "W",
                                                    "Neutral_Wizard");//Task 3: Added the Neutral wizard. aka Rouge wizard as they are in their own faction.
                    units.Add(wr);
                }
            }
            
            for (int j = 0; j < numBuildings; j++)
            {
                if (rd.Next(0, 2) == 0)
                {
                    ResourceBuilding a = new ResourceBuilding(rd.Next(0, 20),
                                                              rd.Next(0, 20),
                                                              500,
                                                              (j % 2 == 0 ? 1 : 0),
                                                              "[RS]");
                    buildings.Add(a);
                }
                else
                {
                    FactoryBuilding f = new FactoryBuilding(rd.Next(0, 20),
                                                              rd.Next(0, 20),
                                                              500,
                                                              (j % 2 == 0 ? 1 : 0),
                                                              "[FS]");
                    buildings.Add(f);
                }
            }
        }
        public void Display(GroupBox groupBox)
        {
            //GenerateBuilding();
            groupBox.Controls.Clear();
            foreach(Unit u in units)
            {
                Button b = new Button();
                if(u is MeleeUnit)
                {
                    MeleeUnit mu = (MeleeUnit)u;
                    b.Size = new Size(20, 20);
                    b.Location = new Point(mu.XPos * 20, mu.YPos * 20);
                    b.Text = mu.Symbol;
                    if(mu.Faction == 0)
                    {
                        b.ForeColor = Color.Red;
                    }
                    else
                    {
                        b.ForeColor = Color.Green;
                    }
                    b.Click += Unit_Click;
                    groupBox.Controls.Add(b);
                }
                else if( u is RangedUnit)
                {
                    RangedUnit ru = (RangedUnit)u;
                    b.Size = new Size(20, 20);
                    b.Location = new Point(ru.XPos * 20, ru.YPos * 20);
                    b.Text = ru.Symbol;
                    if(ru.Faction == 0)
                    {
                        b.ForeColor = Color.Red;
                    }
                    else
                    {
                        b.ForeColor = Color.Green;
                    }
                    b.Click += Unit_Click;
                    groupBox.Controls.Add(b);
                }
                else if(u is WizardUnit)
                {//Displays Wizards
                    WizardUnit wu = (WizardUnit)u;
                    b.Size = new Size(20, 20);
                    b.Location = new Point(wu.XPos * 20, wu.YPos * 20);
                    b.Text = wu.Symbol;
                    if (wu.Faction == 0)
                    {
                        b.ForeColor = Color.Red;
                    }
                    else
                    {
                        b.ForeColor = Color.Green;
                    }
                   
                    b.Click += Unit_Click;
                    groupBox.Controls.Add(b);
                }
                else
                {
                    //Displays Neutral Wizards.
                    Button wb = new Button();
                    RougeWizardUnit Rw = (RougeWizardUnit)u;
                    wb.Size = new Size(20, 20);
                    wb.Location = new Point(Rw.XPos * 20, Rw.YPos * 20);
                    wb.Text = Rw.Symbol;
                    wb.ForeColor = Color.Purple;

                    wb.Click += NeutralFaction_Click;
                    groupBox.Controls.Add(wb);
                }
            }
            foreach (Building d in buildings)
            {
                Button bb = new Button();
                if (d is ResourceBuilding)
                {
                    ResourceBuilding Ab = (ResourceBuilding)d;
                    bb.Size = new Size(40, 40);
                    bb.Location = new Point(Ab.buildingX * 20, Ab.buildingY * 20);
                    bb.Text = Ab.buildingSymbol;
                    if (Ab.buildingFaction == 0)
                    {
                        bb.ForeColor = Color.Red;
                    }
                    else
                    {
                        bb.ForeColor = Color.Green;
                    }
                    bb.Click += Building_Click;
                    groupBox.Controls.Add(bb);
                }
                else
                {
                    FactoryBuilding FB = (FactoryBuilding)d;
                    bb.Size = new Size(40, 40);
                    bb.Location = new Point(FB.buildingX * 20, FB.buildingY * 20);
                    bb.Text = FB.buildingSymbol;
                    if (FB.buildingFaction == 0)
                    {
                        bb.ForeColor = Color.Red;
                    }
                    else
                    {
                        bb.ForeColor = Color.Green;
                    }
                    bb.Click += Building_Click;
                    groupBox.Controls.Add(bb);
                }
            }
        }

        public void Unit_Click(Object sender, EventArgs e)
        {
            int x, y;
            Button b = (Button)sender;
            
            x = b.Location.X / 20;
            y = b.Location.Y / 20;

            foreach (Unit u in units)
            {
                if(u is RangedUnit)
                {
                    RangedUnit ru = (RangedUnit)u;
                    if(ru.XPos == x && ru.YPos == y)
                    {
                        infoTxtBox.Text = "";
                        infoTxtBox.Text = ru.ToString();
                    }
                }
                else if (u is MeleeUnit)
                {
                    MeleeUnit mu = (MeleeUnit)u;
                    if (mu.XPos == x && mu.YPos == y)
                    {
                        infoTxtBox.Text = "";
                        infoTxtBox.Text = mu.ToString();
                    }
                }
                else if (u is WizardUnit)
                {
                    WizardUnit wu = (WizardUnit)u;
                    if (wu.XPos == x && wu.YPos == y)
                    {
                        infoTxtBox.Text = "";
                        infoTxtBox.Text = wu.ToString();
                    }
                }
            }
        }
        //Displays Neutral Wizard Information
        public void NeutralFaction_Click(Object sender, EventArgs e)
        {
            int x, y;
            Button wb = (Button)sender;
            x = wb.Location.X / 20;
            y = wb.Location.Y / 20;
            foreach(Unit rw in units)
            {
                if (rw is RougeWizardUnit)
                {
                    RougeWizardUnit nw = (RougeWizardUnit)rw;
                    if (nw.XPos == x && nw.YPos == y)
                    {
                        infoTxtBox.Text = "";
                        infoTxtBox.Text = nw.ToString();
                    }
                }
            }
        }
        public void Building_Click(Object sender, EventArgs e)
        {
            int x, y;
            Button bb = (Button)sender;
            x = bb.Location.X / 20;
            y = bb.Location.Y / 20;

            foreach (Building d in buildings)
            {
                if (d is ResourceBuilding)
                {
                    ResourceBuilding rB = (ResourceBuilding)d;
                    if (rB.buildingX == x && rB.buildingY == y)
                    {
                        infoTxtBox.Text = "";
                        infoTxtBox.Text = rB.ToString();
                    }
                }
                else if (d is FactoryBuilding)
                {
                    FactoryBuilding fB = (FactoryBuilding)d;
                    if (fB.buildingX == x && fB.buildingY == y)
                    {
                        infoTxtBox.Text = "";
                        infoTxtBox.Text = fB.ToString();
                    }
                }
            }
        }
    }
}
