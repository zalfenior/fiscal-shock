using System.Security.Cryptography;
using ThirdParty.Delaunator;
using System.Collections.Generic;

namespace FiscalShock.Graphs {
    /// <summary>
    /// Interface and extension of the Delaunator library
    /// </summary>
    public class Delaunay {
        public Triangulation triangulation { get; }
        public List<Vertex> vertices { get; private set; }
        public List<Edge> edges { get; private set; }
        public List<Triangle> triangles { get; private set; }

        public Delaunay(List<double> input) {
            triangulation = new Triangulation(input);

            // Set up data structures for use in other scripts
            setTypedGeometry(this);
        }

        /// <summary>
        /// Sets up all geometry from the triangulation into data structures
        /// that are easier to deal with
        /// </summary>
        /// <returns></returns>
        public static void setTypedGeometry(Delaunay dt) {
            dt.vertices = new List<Vertex>();
            dt.edges = new List<Edge>();
            dt.triangles = new List<Triangle>();
            /* The coordinates of triangle t are located at the following
             * indices of dt.coords:
             *    (2*dt.triangles[t],   2*dt.triangles[t] + 1)
             *    (2*dt.triangles[t+1], 2*dt.triangles[t+1] + 1)
             *    (2*dt.triangles[t+2], 2*dt.triangles[t+2] + 1)
             */
            for (int i = 0; i < dt.triangulation.triangles.Count/3; ++i) {
                // TODO do the points need to be in clockwise order?
                Vertex a = new Vertex(
                    dt.triangulation.coords[2*dt.triangulation.triangles[i]],
                    dt.triangulation.coords[2*dt.triangulation.triangles[i] + 1],
                    2*dt.triangulation.triangles[i]
                );
                Vertex b = new Vertex(
                    dt.triangulation.coords[2*dt.triangulation.triangles[i+1]],
                    dt.triangulation.coords[2*dt.triangulation.triangles[i+1] + 1],
                    2*dt.triangulation.triangles[i+1]
                );
                Vertex c = new Vertex(
                    dt.triangulation.coords[2*dt.triangulation.triangles[i+2]],
                    dt.triangulation.coords[2*dt.triangulation.triangles[i+2] + 1],
                    2*dt.triangulation.triangles[i+2]
                );

                /* Link up the vertices. The ids for triangle t's edges are
                 *    3 * t, 3 * t + 1, 3 * t + 2
                 */
                Edge ab = new Edge(a, b, 3*i);
                Edge bc = new Edge(b, c, 3*i+1);
                Edge ac = new Edge(a, c, 3*i+2);
                // TODO add incident edges and adjacent vertices to Vertex objs

                // Add to the Delaunay object
                List<Vertex> vabc = new List<Vertex> { a, b, c };
                List<Edge> eabc = new List<Edge> { ab, bc, ac };
                dt.vertices.AddRange(vabc);
                dt.edges.AddRange(eabc);

                Triangle t = new Triangle(
                    vabc,
                    eabc,
                    i
                );
                dt.triangles.Add(t);
            }
        }
    }
}