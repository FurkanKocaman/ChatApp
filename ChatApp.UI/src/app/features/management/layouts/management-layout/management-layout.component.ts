import { AfterViewChecked, AfterViewInit, Component, OnInit } from "@angular/core";
import { RouterOutlet } from "@angular/router";
import { SidebarComponent } from "../../components/sidebar/sidebar.component";
import { CommonModule } from "@angular/common";
import { ThemeService } from "../../../../core/services/theme.service";

@Component({
  selector: "app-management-layout",
  standalone: true,
  imports: [RouterOutlet, SidebarComponent, CommonModule],
  templateUrl: "./management-layout.component.html",
  styleUrl: "./management-layout.component.css",
})
export class ManagementLayoutComponent implements OnInit {
  isSidebarCollapsed = true;
  isDarkMode: boolean = true;
  currentRouteTitle = "Dashboard";

  constructor(private themeService: ThemeService) {}

  ngOnInit(): void {
    this.themeService.loadTheme();
    this.isDarkMode = this.themeService.isDarkMode();
    this.closeSidebar();
  }

  toggleTheme() {
    this.themeService.toggleTheme();
    this.isDarkMode = !this.isDarkMode;
  }

  openSidebar(event: MouseEvent) {
    event.stopPropagation();
    this.isSidebarCollapsed = true;
  }
  closeSidebar() {
    if (window.innerWidth <= 768) {
      this.isSidebarCollapsed = false;
    }
  }
}
