import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AuthService } from "src/app/services/auth.service";
import { CustomValidators } from "src/app/classes/custom-validators";

@Component({
  selector: "app-registration",
  templateUrl: "./registration.component.html",
  styleUrls: ["./registration.component.scss"],
})
export class RegistrationComponent implements OnInit {
  @Output() onRegistration$ = new EventEmitter<void>();

  public registrationForm: FormGroup;
  public isLoading = false;
  public isSubmitted = false;
  public requestFailed = false;
  public errorMsgs: string[];

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit() {
    this.registrationForm = this.formBuilder.group({
      email: ["", [Validators.required, Validators.email]],
      password: [
        "",
        Validators.compose([
          Validators.required,
          CustomValidators.patternValidator(/\d/, { hasNumber: true }),
          CustomValidators.patternValidator(/[A-Z]/, { hasCapitalCase: true }),
          CustomValidators.patternValidator(/[a-z]/, { hasSmallCase: true }),
          CustomValidators.patternValidator(/[[!@#$%^&*()_+-=[{};':"|,.<>/?]/, {
            hasSpecialCharacters: true,
          }),
        ]),
      ],
    });
  }

  public onRegister() {
    this.isSubmitted = true;
    if (this.registrationForm.invalid) {
      this.errorMsgs = [];
      if (!!this.registrationForm.controls.email.errors.required) {
        this.errorMsgs.push("Email address is required\n");
      } else if (!!this.registrationForm.controls.email.errors.email) {
        this.errorMsgs.push("Incorrect email format\n");
      }
      if (!!this.registrationForm.controls.password.errors.required) {
        this.errorMsgs.push("Password is required\n");
      } else {
        if (!!this.registrationForm.controls.password.errors.hasNumber) {
          this.errorMsgs.push("Password must have a number\n");
        }
        if (!!this.registrationForm.controls.password.errors.hasCapitalCase) {
          this.errorMsgs.push(
            "Password must have at least 1 upper case character\n"
          );
        }
        if (!!this.registrationForm.controls.password.errors.hasSmallCase) {
          this.errorMsgs.push(
            "Password must have at least 1 lower case character\n"
          );
        }
        if (
          !!this.registrationForm.controls.password.errors.hasSpecialCharacters
        ) {
          this.errorMsgs.push(
            "Password must have at least 1 special character\n"
          );
        }
      }
      this.requestFailed = true;
      return;
    }
    this.isLoading = true;
    this.authService
      .register(
        this.registrationForm.controls.email.value,
        this.registrationForm.controls.password.value
      )
      .then(
        (sucess) => {
          this.requestFailed = false;
          this.onRegistration$.emit();
        },
        (reason) => {
          this.errorMsgs = [reason.toString()];
          this.requestFailed = true;
        }
      )
      .finally(() => {
        this.isLoading = false;
      })
      .catch((message: Error) => {
        this.errorMsgs = [message.toString()];
        this.requestFailed = true;
      });
  }
}
