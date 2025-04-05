# 🛠️ Noisy Wallpaper

- 🌐 **API**: Built with **Go** and runs inside a **Docker** container.
- 🖥️ **Service**: A **Windows Service** written in **C#**.

---

## 📦 Installation & Setup

### 1️⃣ Setting Up the API (Go + Docker)
1. Build the Docker image:
   ```sh
   docker build -t noisy-wallpaper-api .
   ```
2. Run the Docker container:
   ```sh
   docker run -d -p 8090:8080
   ```

### 2️⃣ Setting Up the Service (C# Windows Service)
1. Ensure you have **.NET 9 SDK** installed.
2. Navigate to the service folder:
   ```sh
   cd service
   ```
3. Compile the service:
   ```sh
   dotnet publish -c Release -o ./output
   ```
4. Install the service on Windows:
   - Open a **Command Prompt** as Administrator.
   - Run the following command:
     ```sh
     sc create "NoisyWallpaper" binPath="C:\path\to\output\NoisyWallpaper.exe"
     ```
   - Start the service:
     ```sh
     net start NoisyWallpaper
     ```

---

## ⚙️ Configuration
Once the service is installed, you will find a configuration folder named after the service.
Inside, locate the `config.json` file and modify it to fit your needs.

📝 **Example Path:** `C:\ProgramData\NoisyWallpaper\config.json`

---

## 🚀 Usage
Once both the API and service are running, they should work together seamlessly. Make sure to configure the service properly to communicate with the API.

---

## 📜 License
This project is licensed under the [MIT License](LICENSE).

---

## 🛠️ Contributing
Contributions are welcome! Feel free to fork the repository and submit a pull request. 😊

---
