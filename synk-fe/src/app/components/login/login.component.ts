import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { AuthService } from "src/app/services/auth.service";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"],
})
export class LoginComponent implements OnInit {
  public loginForm: FormGroup;
  public isLoading = false;
  public isSubmitted = false;
  public requestFailed = false;
  public errorMsgs: string[];

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      email: ["", Validators.required],
      password: ["", Validators.required],
    });
  }

  public onLogin() {
    this.isSubmitted = true;
    if (this.loginForm.invalid) {
      return;
    }
    this.isLoading = true;
    this.authService
      .login(
        this.loginForm.controls.email.value,
        this.loginForm.controls.password.value
      )
      .then(
        (resolved) => {
          window.location.href = "/home";
        },
        (rejected) => {
          this.requestFailed = true;
          this.errorMsgs = [rejected];
        }
      )
      .finally(() => {
        this.isLoading = false;
      });
  }
}
