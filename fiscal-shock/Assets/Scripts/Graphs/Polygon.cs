using System.Linq;
using System.Collections.Generic;

namespace FiscalShock.Graphs {
    /// <summary>
    /// 2D polygon, defined by its edges (and thereby, vertices)
    /// </summary>
    public class Polygon {
        public List<Edge> sides { get; private set; } = new List<Edge>();
        public List<Vertex> vertices { get; private set; } = new List<Vertex>();

        public Polygon() {}

        public Polygon(List<Edge> boundary) {
            setSides(boundary);
        }

        public void setSides(List<Edge> boundary) {
            sides = boundary;
            vertices = sides.SelectMany(e => e.vertices).Distinct().ToList();
        }
    }

    /// <summary>
    /// A three-sided polygon. What did you expect?
    /// </summary>
    public class Triangle : Polygon {
        public int id { get; }
        public Vertex circumcenter { get; private set; }
        public Vertex a => vertices[0];
        public Vertex b => vertices[1];
        public Vertex c => vertices[2];

        public Triangle(List<Edge> tSides, int tid) {
            setSides(tSides);
            id = tid;
        }

        /// <summary>
        /// Area of the triangle abc is twice the following determinant:
        /// <para />
        /// | a.x  a.y  1 |
        /// | b.x  b.y  1 |
        /// | c.x  c.y  1 |
        ///
        /// <para>Formula derived from Laplacian expansion</para>
        /// </summary>
        /// <returns>area of triangle on given vertices</returns>
        public double getArea() {
            return 2 * (((b.x - a.x) * (c.y - a.y)) - ((b.y - a.y) * (c.x - a.x)));
        }

        /// <summary>
        /// Finds the center point of the circle circumscribed by
        /// this triangle.
        /// Relatively computationally expensive, so save the result
        /// and reuse it in the future.
        /// <para>https://github.com/delfrrr/delaunator-cpp</para>
        /// </summary>
        /// <returns>center point of circumscribed circle</returns>
        public Vertex findCircumcenter() {
            if (circumcenter == null) {
                double dx = b.x - a.x;
                double dy = b.y - a.y;
                double ex = c.x - a.x;
                double ey = c.y - a.y;

                double bl = dx * dx + dy * dy;
                double cl = ex * ex + ey * ey;
                double d = dx * ey - dy * ex;

                double x = vertices[0].x + (ey * bl - dy * cl) * 0.5 / d;
                double y = vertices[0].y + (dx * cl - ex * bl) * 0.5 / d;

                circumcenter = new Vertex(x, y);
            }
            return circumcenter;
        }
    }

    /// <summary>
    /// Voronoi cell extension of base Polygon class
    /// </summary>
    public class Cell : Polygon {
        public Vertex site { get; set; }
        public List<Cell> neighbors { get; set; } = new List<Cell>();
        public int id { get; }

        public Cell(Vertex delaunayVertex) {
            site = delaunayVertex;
            id = site.id;
        }
    }
}