using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class InsectCatchingGame : Form
{
    private Timer gameTimer;
    private List<PictureBox> insects;
    private List<int> insectDisappearTimes; // Thêm danh sách thời gian tự biến mất của côn trùng
    private PictureBox net;
    private int score;
    private int gameTime;
    private Random random;

    private int insectDisappearTime = 5; // Thời gian tự biến mất của côn trùng (5 giây)
    private int maxInsectsPerRound = 5;  // Số lượng côn trùng tối đa xuất hiện mỗi lần

    public InsectCatchingGame()
    {
        // Khởi tạo Form và các biến
        Width = 800;
        Height = 600;
        Text = "Insect Catching Game";

        random = new Random();
        insects = new List<PictureBox>();
        insectDisappearTimes = new List<int>(); // Khởi tạo danh sách thời gian tự biến mất
        score = 0;
        gameTime = 60;

        // Tạo vợt
        net = new PictureBox();
        net.Image = Properties.Resources.net;
        net.Size = new Size(100, 100);
        net.SizeMode = PictureBoxSizeMode.StretchImage;
        net.BackColor = Color.Transparent;
        Controls.Add(net);

        // Khởi tạo Timer cho trò chơi
        gameTimer = new Timer();
        gameTimer.Interval = 1000;
        gameTimer.Tick += UpdateGame;
        gameTimer.Start();

        // Xử lý sự kiện click chuột để bắt côn trùng
        MouseClick += CatchInsects;

        // Tạo và hiển thị côn trùng ban đầu
        GenerateInsects();
    }

    // Hàm tạo và hiển thị côn trùng
    private void GenerateInsects()
    {
        for (int i = 0; i < maxInsectsPerRound; i++)
        {
            PictureBox insect = new PictureBox();
            insect.Image = Properties.Resources.insect;
            insect.Size = new Size(50, 50);
            insect.SizeMode = PictureBoxSizeMode.StretchImage;
            insect.BackColor = Color.Transparent;
            insect.Location = new Point(random.Next(Width - insect.Width), random.Next(Height - insect.Height));
            insects.Add(insect);
            Controls.Add(insect);

            // Gán thời gian tự biến mất của côn trùng
            insectDisappearTimes.Add(insectDisappearTime);
        }
    }

    // Hàm bắt côn trùng khi click chuột
    private void CatchInsects(object sender, MouseEventArgs e)
    {
        foreach (PictureBox insect in insects.ToList())
        {
            if (IsPointInCircle(e.Location, net.Bounds) && IsPointInRectangle(e.Location, insect.Bounds))
            {
                // Xóa côn trùng khi bắt được và cập nhật điểm
                Controls.Remove(insect);
                insects.Remove(insect);
                insectDisappearTimes.RemoveAt(insects.IndexOf(insect));
                score++;
            }
        }
    }

    // Hàm cập nhật trạng thái game
    private void UpdateGame(object sender, EventArgs e)
    {
        gameTime--;

        if (gameTime <= 0)
        {
            // Kết thúc trò chơi khi hết thời gian
            gameTimer.Stop();
            MessageBox.Show($"Game Over! You caught {score} insects.", "Game Over");
            Close();
        }

        MoveInsects();
        CheckInsectDisappear();
    }

    // Hàm di chuyển côn trùng
    private void MoveInsects()
    {
        foreach (PictureBox insect in insects)
        {
            int speed = random.Next(1, 5);
            int directionX = random.Next(-1, 2);
            int directionY = random.Next(-1, 2);

            // Di chuyển côn trùng theo hướng và tốc độ ngẫu nhiên
            insect.Left += directionX * speed;
            insect.Top += directionY * speed;

            // Kiểm tra xem côn trùng có di chuyển ra khỏi màn hình không
            if (insect.Left < 0 || insect.Left > Width - insect.Width || insect.Top < 0 || insect.Top > Height - insect.Height)
            {
                // Đặt lại vị trí nếu di chuyển ra khỏi màn hình
                insect.Location = new Point(random.Next(Width - insect.Width), random.Next(Height - insect.Height));
            }
        }
    }

    // Hàm kiểm tra và xử lý việc côn trùng tự biến mất
    private void CheckInsectDisappear()
    {
        for (int i = 0; i < insects.Count; i++)
        {
            insectDisappearTimes[i]--;

            // Nếu thời gian tự biến mất hết, xóa côn trùng
            if (insectDisappearTimes[i] <= 0)
            {
                Controls.Remove(insects[i]);
                insects.RemoveAt(i);
                insectDisappearTimes.RemoveAt(i);
            }
        }

        // Nếu số lượng côn trùng hiện tại ít hơn số lượng tối đa, tạo thêm
        if (insects.Count < maxInsectsPerRound)
        {
            GenerateInsects();
        }
    }

    // Hàm di chuyển vợt theo chuột
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        net.Location = new Point(e.X - net.Width / 2, e.Y - net.Height / 2);
    }

    // Hàm kiểm tra điểm của côn trùng
    private bool IsPointInRectangle(Point point, Rectangle rectangle)
    {
        return rectangle.Contains(point);
    }

    // Hàm kiểm tra vị trí điểm có nằm trong vòng tròn hay không
    private bool IsPointInCircle(Point point, Rectangle rectangle)
    {
        float centerX = rectangle.Left + rectangle.Width / 2.0f;
        float centerY = rectangle.Top + rectangle.Height / 2.0f;
        float radius = rectangle.Width / 2.0f;

        double distance = Math.Sqrt(Math.Pow(point.X - centerX, 2) + Math.Pow(point.Y - centerY, 2));

        return distance <= radius;
    }

    // Hàm main
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new InsectCatchingGame());
    }
}