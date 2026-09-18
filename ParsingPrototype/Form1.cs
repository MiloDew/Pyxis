using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace ParsingPrototype
{
    public partial class FormMain : Form
    {
        private Image _mapImage;
        private double _tlEasting, _tlNorthing, _brEasting, _brNorthing;
        private const double CellSizeMetres = 5.0;
        private List<GridCell> _grid = new List<GridCell>();
        private Graph _graph;

        private int _columnCount;
        private int _rowCount;
        private double _pixelsPerCellX;
        private double _pixelsPerCellY;
        private List<GridCell> _placedControls = new List<GridCell>();
        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonLoadMap_Click(object sender, EventArgs e)
        {
            if (openFileDialogMap.ShowDialog() != DialogResult.OK)
                return;

            byte[] bytes = File.ReadAllBytes(openFileDialogMap.FileName);
            using (var stream = new MemoryStream(bytes))
            {
                _mapImage = Image.FromStream(stream);
            }
            pictureBoxMap.Image = _mapImage;

            _grid.Clear();
            listBoxControls.Items.Clear();
        }

        private void buttonApplyCorners_Click(object sender, EventArgs e)
        {
            if (_mapImage == null)
            {
                MessageBox.Show("Load a map first.");
                return;
            }

            bool parsedOk = double.TryParse(textBoxTLEasting.Text, out _tlEasting)
                         && double.TryParse(textBoxTLNorthing.Text, out _tlNorthing)
                         && double.TryParse(textBoxBREasting.Text, out _brEasting)
                         && double.TryParse(textBoxBRNorthing.Text, out _brNorthing);

            if (!parsedOk)
            {
                MessageBox.Show("Enter valid numbers in all four corner boxes.");
                return;
            }

            BuildGrid();
        }

        private void BuildGrid()
        {

            double metresPerPixelX = Math.Abs(_brEasting - _tlEasting) / _mapImage.Width;
            double metresPerPixelY = Math.Abs(_brNorthing - _tlNorthing) / _mapImage.Height;

            double pixelsPerCellX = CellSizeMetres / metresPerPixelX;
            double pixelsPerCellY = CellSizeMetres / metresPerPixelY;

            long columns = (long)(_mapImage.Width / pixelsPerCellX);
            long rows = (long)(_mapImage.Height / pixelsPerCellY);

            const long MaxSensibleCells = 500_000;
            if (columns <= 0 || rows <= 0 || columns * rows > MaxSensibleCells)
            {
                MessageBox.Show(
                    $"That would create {columns} x {rows} cells, which doesn't look right for a map this size. " +
                    "Check your corner coordinates.");
                return;
            }

            int colCount = (int)columns;
            int rowCount = (int)rows;

            _grid.Clear();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    double pixelX = (col + 0.5) * pixelsPerCellX;
                    double pixelY = (row + 0.5) * pixelsPerCellY;

                    double fracX = pixelX / _mapImage.Width;
                    double fracY = pixelY / _mapImage.Height;
                    double easting = _tlEasting + fracX * (_brEasting - _tlEasting);
                    double northing = _tlNorthing + fracY * (_brNorthing - _tlNorthing);

                    _grid.Add(new GridCell
                    {
                        Row = row,
                        Column = col,
                        PixelX = pixelX,
                        PixelY = pixelY,
                        Easting = easting,
                        Northing = northing
                    });
                }
            }
            _pixelsPerCellX = pixelsPerCellX;
            _pixelsPerCellY = pixelsPerCellY;
            _columnCount = colCount;
            _rowCount = rowCount;

            _graph = GraphBuilder.BuildGraphFromGrid(_grid);
            MessageBox.Show($"Grid built: {columns} x {rows} = {_grid.Count} cells.");
        }

        private void pictureBoxMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (_mapImage == null || _grid.Count == 0)
                return;

            // Convert the click's on-screen pixel position into the image's
            // own pixel coordinates, since StretchImage scales the whole
            // image to fit the fixed-size box.
            double scaleX = _mapImage.Width / (double)pictureBoxMap.Width;
            double scaleY = _mapImage.Height / (double)pictureBoxMap.Height;
            double imagePixelX = e.X * scaleX;
            double imagePixelY = e.Y * scaleY;

            // Because the grid is a regular lattice, you don't need to search
            // every cell to find the nearest one - just divide the pixel
            // position by the cell size to get its row/column directly.
            int col = (int)(imagePixelX / _pixelsPerCellX);
            int row = (int)(imagePixelY / _pixelsPerCellY);

            // Clamp in case the click lands exactly on the image's far edge,
            // which would otherwise compute an out-of-range column/row.
            col = Math.Max(0, Math.Min(col, _columnCount - 1));
            row = Math.Max(0, Math.Min(row, _rowCount - 1));

            int index = row * _columnCount + col;
            GridCell nearest = _grid[index];

            _placedControls.Add(nearest);
            listBoxControls.Items.Add(
                $"Control {_placedControls.Count}: row {nearest.Row}, col {nearest.Column} " +
                $"(E {nearest.Easting:F1}, N {nearest.Northing:F1})");

            pictureBoxMap.Invalidate(); // triggers the Paint handler to run again
        }

        private void pictureBoxMap_Paint(object sender, PaintEventArgs e)
        {
            if (_mapImage == null || _placedControls.Count == 0)
                return;

            // Same scale factor as the click handler, but inverted - now
            // going from the image's pixel space back to screen pixels.
            double drawScaleX = pictureBoxMap.Width / (double)_mapImage.Width;
            double drawScaleY = pictureBoxMap.Height / (double)_mapImage.Height;

            for (int i = 0; i < _placedControls.Count; i++)
            {
                GridCell cell = _placedControls[i];
                float screenX = (float)(cell.PixelX * drawScaleX);
                float screenY = (float)(cell.PixelY * drawScaleY);

                e.Graphics.FillEllipse(Brushes.Red, screenX - 5, screenY - 5, 10, 10);
                e.Graphics.DrawString((i + 1).ToString(), Font, Brushes.White, screenX + 6, screenY - 8);
            }
        }

        private void buttonFindRoute_Click(object sender, EventArgs e)
        {
            if (_placedControls.Count < 2)
            {
                MessageBox.Show("Place at least two controls first.");
                return;
            }

            GridCell startCell = _placedControls[0];
            GridCell endCell = _placedControls[1];

            Node startNode = _graph.Nodes.First(n => n.Row == startCell.Row && n.Column == startCell.Column);
            Node endNode = _graph.Nodes.First(n => n.Row == endCell.Row && n.Column == endCell.Column);

            List<Node> path = _graph.Dijkstra(startNode, endNode);

            if (path.Count == 0)
            {
                MessageBox.Show("No route found between those two controls.");
                return;
            }

            string routeText = string.Join(" -> ", path.Select(n => n.Name));
            MessageBox.Show(routeText, "Route");
        }

        private async void buttonTest_Click(object sender, EventArgs e)
        {
            // Ben Nevis summit - UK's highest point (~1345m), an easy sanity check.
            double testLat = 56.79685;
            double testLon = -5.00355;

            try
            {
                double? elevation = await ElevationClient.GetElevationAsync(testLat, testLon);

                if (elevation.HasValue)
                    MessageBox.Show($"Elevation: {elevation.Value} m");
                else
                    MessageBox.Show("API returned no data for that point.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Request failed: {ex.Message}");
            }
        }
    }
}
