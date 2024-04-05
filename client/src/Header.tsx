

function Header() {
    return (
        <>
            <div className="d-flex justify-content-between bg-secondary py-2 px-4">
                <span>
                    Anki Books
                </span>

                <span>
                    <a href="#">
                        Register
                    </a>

                    <a href="#">
                        Login
                    </a>
                </span>
            </div>
        </>
    )
}

export default Header;