import { Component } from "@angular/core";
import { AuthService } from "./services/auth.service";
import { Router } from "@angular/router";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"],
})
export class AppComponent {
  title = "synk-fe";
  userLoading = true;
  error: string = undefined;

  constructor(public authService: AuthService, private router: Router) {
    authService
      .sessionStorageAuthentication()
      .finally(() => {
        this.userLoading = false;
      })
      .catch((error: string) => {
        this.error = error;
        router.navigate(["login"]);
      });
  }
}
