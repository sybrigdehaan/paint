using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Ink;

namespace paint
{
    public partial class MainWindow : Window
    {
        private Items currentItem;
        private bool drawn = false;
        public static double x1, y1, x2, y2;

        public static SolidColorBrush mySolidColorBrushRed = new SolidColorBrush();
        public static UndoRedoManager undoRedoManager = new UndoRedoManager();
        public static SaveLoadManager saveLoadManager = new SaveLoadManager(); 

        public static InkCanvas myInkCanvas = MyInkCanvas.GetInstance();
        public static _Group myMainGroup = MyMainGroup.GetInstance();

        private ICustomObjectVisitor visitor = new CustumObjectVisitor();
        private SimpleRemoteControl remote = new SimpleRemoteControl();

        private _Ellipse myEllipse;
        private _Rectangle myRectangle;

        public MainWindow()
        {
            InitializeComponent();
            myGrid.Children.Add(myInkCanvas);
            myInkCanvas.EditingMode = InkCanvasEditingMode.None;
            mySolidColorBrushRed.Color = Color.FromArgb(255, 255, 0, 0);
        }

        public enum Items { None, OpenFile, SaveFile, DeleteGroup, AddGroup, Select, Eraser, Ornament, Rectangle, Ellipse }

        private void Button_MakeFigure_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            myInkCanvas.EditingMode = InkCanvasEditingMode.None;
            switch (((FrameworkElement)sender).Name)
            {
                case "Rectangle":
                    currentItem = Items.Rectangle;
                    break;
                case "Ellipse":
                    currentItem = Items.Ellipse;
                    break;
            }
        }

        private void Button_ChangeFile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (((FrameworkElement)sender).Name)
            {
                case "Load":
                    saveLoadManager.Load(); 
                    break;
                case "Save":
                    saveLoadManager.Save(); 
                    break;
                case "Redo":
                    undoRedoManager.Redo();
                    break;
                case "Undo":
                    undoRedoManager.Undo();
                    break;
            }
        }

        private void Button_ChangeFigure_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentItem = Items.None;
            FrameworkElement[] myArray = new FrameworkElement[myInkCanvas.GetSelectedElements().Count];
            myInkCanvas.GetSelectedElements().CopyTo(myArray, 0);

            switch (((FrameworkElement)sender).Name)
            {
                case "Delete_Group":
                    _Group selectedGroup = new _Group();
                    foreach (IFigures figure in myMainGroup.SubFigures)
                    {
                        if (typeof(_Group) == figure.GetType())
                        {
                            if (figure.GetShape() == myArray[0])
                                selectedGroup = ((figure as _Group));
                        }
                    }

                    remote = new SimpleRemoteControl { SetCommand = new _DestroyGroup(selectedGroup) };
                    remote.buttonWasPressed();
                    undoRedoManager.AddToUndo(remote);

                    remote = new SimpleRemoteControl { SetCommand = new _MakeGroup(selectedGroup, selectedGroup.SubFigures) };
                    undoRedoManager.AddToRedo(remote);
                    break;
                case "Add_Group":
                    List<IFigures> selectedFigures = new List<IFigures>(); 
                    for (int i = 0; i < myArray.Length; i++)
                    {
                        foreach (IFigures figure in myMainGroup.SubFigures)
                        {
                            if (figure.GetShape() == myArray[i])
                                selectedFigures.Add(figure); 
                        }
                    }
            
                    _Group myGroup = new _Group();
                    remote = new SimpleRemoteControl { SetCommand = new _MakeGroup(myGroup, selectedFigures) };
                    remote.buttonWasPressed();
                    undoRedoManager.AddToRedo(remote);

                    remote = new SimpleRemoteControl { SetCommand = new _DestroyGroup(myGroup) };
                    undoRedoManager.AddToUndo(remote);
                    break;
                case "Select":
                    myInkCanvas.EditingMode = InkCanvasEditingMode.Select;
                    break;
                case "Eraser":
                    for (int i = 0; i < myArray.Length; i++)
                    {
                        IFigures eraserSelectedFigure = null;
                        foreach (IFigures figure in myMainGroup.SubFigures)
                        {
                            if (figure.GetShape() == myArray[i])
                                eraserSelectedFigure = figure;
                        }

                        if (typeof(_Group) == eraserSelectedFigure.GetType())
                        {
                            remote = new SimpleRemoteControl { SetCommand = new _DestroyGroup((eraserSelectedFigure as _Group)) };
                            remote.buttonWasPressed();
                        }

                        else
                        {
                            remote = new SimpleRemoteControl { SetCommand = new _DestroyShape(eraserSelectedFigure) };
                            remote.buttonWasPressed();

                            remote = new SimpleRemoteControl { SetCommand = new _MakeShape(eraserSelectedFigure) };
                            undoRedoManager.AddToUndo(remote);
                        }
                    }
                    break;
            }
        }

        private void Button_AddToFigure_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            // haalt alle geslecteerde items op en zet het in array Shape
            FrameworkElement[] myArray = new FrameworkElement[myInkCanvas.GetSelectedElements().Count];
            myInkCanvas.GetSelectedElements().CopyTo(myArray, 0);

            List<IFigures> _ShapesList = new List<IFigures>();
            //myMainGroup.Get_Shape(ref _ShapesList);

            IFigures selectedFigure = null;
            foreach (IFigures figure in _ShapesList)
            {
                if (figure.GetShape() == myArray[0])
                    selectedFigure = figure;
            }

            // haalt naam op van de het aangeklikte element
            switch (((FrameworkElement)sender).Name)
            {
                case "OrnamentRight":
                    double ornamentRightLeft = InkCanvas.GetLeft(myArray[0]);
                    double ornamentRightTop = InkCanvas.GetTop(myArray[0]);
                    double OrnamentRightHeigth = myArray[0].Height / 2;
                    double OrnamentRightWidht = myArray[0].Width;
                    Right ornamentRight = new Right("ornament", (ornamentRightLeft + OrnamentRightWidht), (OrnamentRightHeigth + ornamentRightTop));
                    ornamentRight.Add(ref myInkCanvas, selectedFigure);
                    break;

                case "OrnamentLeft":
                    double OrnamentLeftLeft = InkCanvas.GetLeft(myArray[0]);
                    double OrnamentLeftTop = InkCanvas.GetTop(myArray[0]);
                    double OrnamentLeftHeigth = myArray[0].Height / 2;
                    Left ornamentLeft = new Left("ornament", OrnamentLeftLeft, (OrnamentLeftHeigth + OrnamentLeftTop));
                    ornamentLeft.Add(ref myInkCanvas, selectedFigure);
                    break;

                case "OrnamentTop":
                    double OrnamentTopRight = InkCanvas.GetLeft(myArray[0]);
                    double ornamentTopTop = InkCanvas.GetTop(myArray[0]);
                    double OrnamentTopWidth = myArray[0].Width / 2;
                    Top ornamenTtop = new Top("ornament", (OrnamentTopWidth + OrnamentTopRight), ornamentTopTop);
                    ornamenTtop.Add(ref myInkCanvas, selectedFigure);
                    break;

                case "OrnamentBottom":
                    double OrnamentBottomRight = InkCanvas.GetLeft(myArray[0]);
                    double ornamentBottomTop = InkCanvas.GetTop(myArray[0]);
                    double OrnamentBottomWidth = myArray[0].Width / 2;
                    double OrnamentBottomHeight = myArray[0].Height;
                    Bottom OrnamentBottom = new Bottom("ornament", (ornamentBottomTop + OrnamentBottomWidth), (ornamentBottomTop + OrnamentBottomHeight));
                    OrnamentBottom.Add(ref myInkCanvas, selectedFigure);
                    break;
            }
        }

        private void InkCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            drawn = true;
            Point p = e.GetPosition(myInkCanvas);
            x1 = p.X; y1 = p.Y;

            switch (currentItem)
            {
                case Items.Rectangle:
                    myRectangle = new _Rectangle();
                    remote = new SimpleRemoteControl { SetCommand = new _MakeShape(myRectangle) };
                    remote.buttonWasPressed();
                    undoRedoManager.AddToRedo(remote);
                    break;
                case Items.Ellipse:
                    myEllipse = new _Ellipse();
                    remote = new SimpleRemoteControl { SetCommand = new _MakeShape(myEllipse) };
                    remote.buttonWasPressed();
                    undoRedoManager.AddToRedo(remote);
                    break;
            }
        }

        private void InkCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            drawn = false;
            switch (currentItem)
            {
                case Items.Rectangle:
                    remote = new SimpleRemoteControl { SetCommand = new _DestroyShape(myRectangle) };
                    undoRedoManager.AddToUndo(remote);
                    break;
                case Items.Ellipse:
                    remote = new SimpleRemoteControl { SetCommand = new _DestroyShape(myEllipse) };
                    undoRedoManager.AddToUndo(remote);
                    break;
            }
        }

        private void InkCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(myInkCanvas);
            x2 = p.X; y2 = p.Y;

            if (drawn)
            {
                switch (currentItem)
                {
                    case Items.Rectangle:
                        myRectangle.ChangeFigure(x1, x2, y1, y2);
                        break;
                    case Items.Ellipse:
                        myEllipse.ChangeFigure(x1, x2, y1, y2);
                        break;
                }
            }
        }
    }
}
