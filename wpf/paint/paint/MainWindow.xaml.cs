using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Ink;
using System.Linq;

namespace paint
{
    public partial class MainWindow : Window
    {
        private Items currentItem;
        private bool drawn = false;
        private double x1, y1, x2, y2;

        public static SolidColorBrush mySolidColorBrushRed = new SolidColorBrush();
        public static UndoRedoManager undoRedoManager = new UndoRedoManager();
        public static SaveLoadManager saveLoadManager = new SaveLoadManager();
        public static IAddRemoveVisitor addRemoveVisitor = new AddRemoveVisitor();

        private _Group myMainGroup = MyMainGroup.GetInstance();
        private InkCanvas myInkCanvas = null; 
        private SimpleRemoteControl remote = new SimpleRemoteControl();
        private _Ellipse myEllipse;
        private _Rectangle myRectangle;

        public MainWindow()
        {
            myInkCanvas = (myMainGroup.GetShape() as InkCanvas);
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

                    remote = new SimpleRemoteControl { SetCommand = new _UnGroup(selectedGroup) };
                    remote.buttonWasPressed();
                    undoRedoManager.AddToUndo(remote);

                    remote = new SimpleRemoteControl { SetCommand = new _EnGroup(selectedGroup, selectedGroup.SubFigures) };
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
                    remote = new SimpleRemoteControl { SetCommand = new _EnGroup(myGroup, selectedFigures) };
                    remote.buttonWasPressed();
                    undoRedoManager.AddToRedo(remote);

                    remote = new SimpleRemoteControl { SetCommand = new _UnGroup(myGroup) };
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
                        try
                        {
                            if (typeof(_Group) == eraserSelectedFigure.GetType())
                            {
                                SimpleRemoteControl enRemote = new SimpleRemoteControl
                                {
                                    SetCommand = new _EnGroup((eraserSelectedFigure as _Group),
                                   (eraserSelectedFigure as _Group).SubFigures.ToList())
                                };
                                undoRedoManager.AddToUndo(enRemote);

                                SimpleRemoteControl deRemote = new SimpleRemoteControl { SetCommand = new _DestroyGroup((eraserSelectedFigure as _Group)) };
                                deRemote.buttonWasPressed();
                                undoRedoManager.AddToRedo(deRemote);
                            }

                            else
                            {
                                remote = new SimpleRemoteControl { SetCommand = new _DestroyShape(eraserSelectedFigure) };
                                remote.buttonWasPressed();

                                remote = new SimpleRemoteControl { SetCommand = new _MakeShape(eraserSelectedFigure) };
                                undoRedoManager.AddToUndo(remote);
                            }
                        }
                        catch
                        {
                            (MyMainGroup.GetInstance().GetShape() as InkCanvas).Children.Remove(myArray[i]); 
                        }
                    }
                    break;
            }
        }

        private void Button_AddToFigure_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            // Haalt alle geslecteerde items op en zet het in array Shape
            FrameworkElement[] myArray = new FrameworkElement[myInkCanvas.GetSelectedElements().Count];
            myInkCanvas.GetSelectedElements().CopyTo(myArray, 0);

            IFigures selectedFigure = null;
            foreach (IFigures figure in myMainGroup.SubFigures)
            {
                if (figure.GetShape() == myArray[0])
                    selectedFigure = figure;
            }

            // Haalt naam op van de het aangeklikte element
            switch (((FrameworkElement)sender).Name)
            {
                case "OrnamentLeft":
                    selectedFigure = new Left(selectedFigure);
                    break;
                case "OrnamentRight":
                    selectedFigure = new Right(selectedFigure); 
                    break;
                case "OrnamentTop":
                    selectedFigure = new Top(selectedFigure);
                    break;
                case "OrnamentBottom":
                    selectedFigure = new Bottom(selectedFigure);
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
