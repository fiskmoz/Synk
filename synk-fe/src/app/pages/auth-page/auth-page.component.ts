import { Component, OnInit } from "@angular/core";
import { AuthService } from "src/app/services/auth.service";

@Component({
  selector: "app-auth-page",
  templateUrl: "./auth-page.component.html",
  styleUrls: ["./auth-page.component.scss"],
})
export class AuthPageComponent implements OnInit {
  public loginMode = true;
  public hasRegistered = false;
  constructor(authService: AuthService) {}

  ngOnInit() {}

  public toggle() {
    this.loginMode = !this.loginMode;
  }

  public onRegistration() {
    setTimeout(() => {
      this.loginMode = true;
    }, 200);
    this.hasRegistered = true;
  }
}
