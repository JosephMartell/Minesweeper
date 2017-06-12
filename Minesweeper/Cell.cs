using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Minesweeper {
    class Cell {
        public Point Location { get; }
        public bool Mine { get; set; }
        public bool Revealed { get; private set; }
        public int Width { get; }
        public void Reveal() {
            Revealed = true;
        }

        public int Count { get; set; }

        private Point TopLeft() {
            return new Point(Location.X * Width, Location.Y * Width);
        }

        private Rectangle DrawArea() {
            return new Rectangle(TopLeft(), new Size(Width, Width));
        }

        public Cell(Point location, int width) {
            Location = location;
            Width = width;
            Mine = false;
        }


        public void Paint(Graphics g) {
            if (Revealed) {
                if (Mine) {
                    g.FillRectangle(Brushes.Red, DrawArea());
                }
                else {
                    if (Count > 0) {
                        g.DrawString(this.Count.ToString(), new Font("Arial", 12), Brushes.Black, DrawArea());
                    }
                }
            }
            else {
                g.FillRectangle(Brushes.Gray, DrawArea());
            }
            g.DrawRectangle(Pens.Black, DrawArea());
        }

        public bool Contains(Point clickSpot) {
            return DrawArea().Contains(clickSpot);
        }

        public IEnumerable<Point> Neighbors(int colCount, int rowCount) {
            List<Point> neighbors = new List<Point>();
            for (int i = -1; i < 2; i++) {
                for (int j = -1; j < 2; j++) {
                    int newI = Location.X + i;
                    int newJ = Location.Y + j;

                    if ((newI >= 0) && (newI < colCount) && (newJ >= 0) && (newJ < rowCount)) {
                        neighbors.Add(new Point(newI, newJ));
                    }
                }

            }

            return neighbors;
        }
    }
}
