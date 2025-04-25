import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root",
})
export class ThemeService {
  private readonly storageKey = "theme";
  private readonly darkClass = "dark";

  constructor() {
    this.loadTheme();
  }

  toggleTheme(): void {
    const isDark = document.documentElement.classList.contains(this.darkClass);
    if (isDark) this.setLightTheme();
    else this.setDarkTheme();
  }

  setDarkTheme(): void {
    document.documentElement.classList.add(this.darkClass);
    localStorage.setItem(this.storageKey, this.darkClass);
  }

  setLightTheme(): void {
    document.documentElement.classList.remove(this.darkClass);
    localStorage.setItem(this.storageKey, "light");
  }

  loadTheme(): void {
    const savedTheme = localStorage.getItem(this.storageKey);
    if (savedTheme === this.darkClass) this.setDarkTheme();
    else this.setLightTheme();
  }
  isDarkMode(): boolean {
    return document.documentElement.classList.contains(this.darkClass);
  }
}
