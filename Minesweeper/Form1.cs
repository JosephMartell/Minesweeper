using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper {
    public partial class Form1 : Form {
        private int rowCount = 20;
        private int colCount = 20;
        private int width = 20;
        private int mineCount = 40;
        private List<Cell> cells;

        private Cell[,] gameBoard;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            SetBoard();
        }

        private void SetBoard() {
            cells = new List<Cell>();
            Cell[] test = new Cell[100];
            gameBoard = new Cell[colCount, rowCount];
            for (int i = 0; i < colCount; i++) {
                for (int j = 0; j < rowCount; j++) {
                    gameBoard[i, j] = new Cell(new Point(i, j), width);
                    cells.Add(gameBoard[i, j]);
                }
            }

            cells.Shuffle();
            var MineCells = cells.Take(mineCount);
            foreach (var c in MineCells) {
                c.Mine = true;
            }
            SetCellCounts();

        }

        private void Form1_Paint(object sender, PaintEventArgs e) {
            for (int i = 0; i < colCount; i++) {
                for (int j = 0; j < rowCount; j++) {
                    gameBoard[i, j].Paint(e.Graphics);
                }
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e) {
            for (int i = 0; i < colCount; i++) {
                for (int j = 0; j < rowCount; j++) {
                    if (gameBoard[i, j].Contains(e.Location)) {
                        if (!gameBoard[i,j].Revealed) {
                            gameBoard[i, j].Reveal();
                            if (gameBoard[i, j].Count == 0) {
                                CascadeReveal(gameBoard[i, j].Neighbors(colCount, rowCount));
                            }
                            if (gameBoard[i,j].Mine) {
                                RevealBoard();
                            }
                        }
                        break;
                    }
                }
            }
            this.Invalidate();
        }

        private void SetCellCounts() {
            for (int i = 0; i < colCount; i++) {
                for (int j = 0; j < rowCount; j++) {
                    for (int iMod = -1; iMod <= 1; iMod++) {
                        for (int jMod = -1; jMod <= 1; jMod++) {
                            int colIndex = i + iMod;
                            int rowIndex = j + jMod;
                            if ((colIndex >= 0) && (colIndex < colCount) &&
                                (rowIndex >= 0) && (rowIndex < rowCount)) {
                                if (gameBoard[colIndex, rowIndex].Mine) {
                                    gameBoard[i, j].Count++;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CascadeReveal(IEnumerable<Point> cells) {
            foreach (var c in cells) {
                Cell targetCell = gameBoard[c.X, c.Y];
                if (!targetCell.Mine && !targetCell.Revealed) {
                    targetCell.Reveal();
                    if (targetCell.Count <= 0) {
                        CascadeReveal(targetCell.Neighbors(colCount, rowCount));
                    }
                }
            }
        }


        private void RevealBoard() {
            for (int i = 0; i < colCount; i++) {
                for (int j = 0; j < rowCount; j++) {
                    gameBoard[i, j].Reveal();
                }
            }
        }

        private void btnResetBoard_Click(object sender, EventArgs e) {
            SetBoard();
            Invalidate();
        }
    }
}
